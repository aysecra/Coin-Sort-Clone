using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoinSortClone.Component;
using CoinSortClone.Data;
using CoinSortClone.Manager;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace CoinSortClone.Logic
{
    public static class SlotController
    {
        private static Dictionary<Slot, Stack<Coin>> _slotDictionary = new Dictionary<Slot, Stack<Coin>>();
        private static Stack<Coin> _lastCoinStack = new Stack<Coin>();
        private static List<Slot> _slotList = new List<Slot>();
        private static Coin _nullCoin = null;
        private static Coin _lastCoin;
        private static int _firstEmptySlotIndex = -1;

        public static void AddSlot(Slot slot, bool isEnable)
        {
            if (_slotDictionary.ContainsKey(slot)) return;

            if (!_slotList.Contains(slot))
                _slotList.Add(slot);

            if (!isEnable) return;

            _slotDictionary[slot] = new Stack<Coin>();
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

            _slotList.Sort(CompareSlot);

            for (int i = 0; i < _slotList.Count; i++)
            {
                if (_slotList[i].IsEnable) continue;
                _firstEmptySlotIndex = i;
                ProgressManager.Instance.SetSlotIndex(i);
                break;
            }

            if (_firstEmptySlotIndex >= 0)
            {
                _slotList[_firstEmptySlotIndex].ShowSlotPrice(ProgressManager.Instance.CalculateSlotPrice());
            }
        }

        public static void IncreaseSlotIndex()
        {
            _firstEmptySlotIndex++;
            ProgressManager.Instance.SetSlotIndex(_firstEmptySlotIndex);
            _slotList[_firstEmptySlotIndex].ShowSlotPrice(ProgressManager.Instance.CalculateSlotPrice());
        }

        private static int CompareSlot(Slot a, Slot b)
        {
            Vector3 va = a.Border.Center;
            Vector3 vb = b.Border.Center;

            if (Mathf.Abs(va.z - vb.z) < 0.001f)
            {
                if (Mathf.Abs(va.x - vb.x) < 0.001f) return 0;
                return va.x < vb.x ? -1 : 1;
            }

            return va.z < vb.z ? -1 : 1;
        }

        public static Coin PopFromSlot(Slot slot)
        {
            return _slotDictionary[slot].Pop();
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
            foreach (var slot in _slotDictionary.Keys.Where(slot => slot.IsMergeable))
            {
                CoinMovement.Instance.Merge(slot, GetLastCoinStackFromSlot(slot));
            }
        }

        public static void RemoveLastCoin(Slot slot)
        {
            Coin coin = PopFromSlot(slot);
            coin.Transform.gameObject.SetActive(false);
            SharedLevelManager.Instance.GetBackCoin(coin);
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

        public static Stack<Coin> GetLastCoinStackFromSlot(Slot slot, int targetCount = 0)
        {
            _lastCoinStack.Clear();

            if (_slotDictionary[slot].Count == 0)
                return _lastCoinStack;

            Coin last = _slotDictionary[slot].Peek();
            int counter = 0;

            foreach (var current in _slotDictionary[slot].TakeWhile(current => current.IsEqual(last)))
            {
                if (targetCount != 0 && counter == targetCount)
                    break;

                _lastCoinStack.Push(current);
                counter++;
            }

            return _lastCoinStack;
        }

        public static Coin GetLastCoin(Slot slot)
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