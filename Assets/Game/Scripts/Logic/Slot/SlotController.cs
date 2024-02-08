using System.Collections.Generic;
using System.Linq;
using CoinSortClone.Component;
using CoinSortClone.Data;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace CoinSortClone.Logic
{
    public static class SlotController
    {
        private static Dictionary<Slot, Stack<Coin>> _slotDictionary = new Dictionary<Slot, Stack<Coin>>();
        private static Dictionary<Slot, Stack<Coin>> _emptySlotDictionary = new Dictionary<Slot, Stack<Coin>>();
        private static Stack<Coin> _lastCoinStack = new Stack<Coin>();
        private static List<Slot> _slotList = new List<Slot>();
        private static Coin _nullCoin = null;

        public static void AddSlot(Slot slot, bool isEnable)
        {
            if (_slotDictionary.ContainsKey(slot)) return;

            if (!_slotList.Contains(slot))
                _slotList.Add(slot);
            
            if (isEnable)
            {
                _slotDictionary[slot] = new Stack<Coin>();
                _emptySlotDictionary[slot] = _slotDictionary[slot];
            }
        }

        public static void AddCoinToSlot(Slot slot, Coin coin)
        {
            _slotDictionary[slot].Push(coin);
        }

        private static async void OrderSlot()
        {
            await Task.Delay(20);

            // _slotList = _slotList.OrderBy(slot => slot.Border.Center.z)
            //     .ThenBy(slot => slot.Border.Center.x).ToList();

            _slotList = _slotList.OrderBy(slot => slot.Border.Center.x).ToList();
            _slotList = _slotList.OrderBy(slot => slot.Border.Center.z).ToList();
        }

        public static void Move(Slot selected, Slot target)
        {
            Coin lastCoin = GetLastCoin(target);
            if (!GetLastCoin(selected).IsEqual(lastCoin) && lastCoin != _nullCoin)
                return;

            Stack<Coin> coins = GetLastCoinStackFromSlot(selected);
            int direction = target.transform.position.x >= selected.transform.position.x ? 1 : -1;
            target.Move(coins, direction);
            selected.DecreaseCoin(coins.Count);
            for (int i = 0; i < coins.Count; i++)
            {
                _slotDictionary[selected].Pop();
            }
        }

        public static void Deal()
        {
            Slot emptySlot = _slotDictionary.ElementAt(Random.Range(0, _slotDictionary.Count)).Key;

            foreach (var slot in _slotDictionary.Keys.Where(slot => emptySlot.GetHashCode() != slot.GetHashCode()))
            {
                slot.AddCoin();
            }
        }

        public static void Merge()
        {
        }

        public static bool MergeControl(Slot slot)
        {
            Coin last = GetLastCoin(slot);
            
            foreach (var coin in _slotDictionary[slot])
            {
                if (!last.IsEqual(coin)) return false;
            }

            return true;
        }

        public static Stack<Coin> GetLastCoinStackFromSlot(Slot slot)
        {
            _lastCoinStack.Clear();

            if (_slotDictionary[slot].Count == 0)
                return _lastCoinStack;

            Coin last = _slotDictionary[slot].Peek();

            foreach (var current in _slotDictionary[slot].TakeWhile(current => current.IsEqual(last)))
            {
                _lastCoinStack.Push(current);
            }

            return _lastCoinStack;
        }

        private static Coin GetLastCoin(Slot slot)
        {
            return _slotDictionary[slot].Count > 0 ? _slotDictionary[slot].Peek() : _nullCoin;
        }

        public static void Clear()
        {
            _slotDictionary.Clear();
            _lastCoinStack.Clear();
            _slotList.Clear();
            OrderSlot();
        }
    }
}