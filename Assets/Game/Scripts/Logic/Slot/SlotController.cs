using System.Collections.Generic;
using System.Linq;
using CoinSortClone.Component;

namespace CoinSortClone.Logic
{
    public static class SlotController
    {
        private static Dictionary<Slot, Stack<Coin>> _slotDictionary = new Dictionary<Slot, Stack<Coin>>();
        private static Stack<Coin> _lastCoinStack = new Stack<Coin>();

        public static void AddSlot(Slot slot)
        {
            if (!_slotDictionary.ContainsKey(slot))
                _slotDictionary[slot] = new Stack<Coin>();
        }

        public static void AddCoinToSlot(Slot slot, Coin coin)
        {
            _slotDictionary[slot].Push(coin);
        }

        public static Stack<Coin> GetLastCoinStackFromSlot(Slot slot)
        {
            _lastCoinStack.Clear();

            Coin last = _slotDictionary[slot].Peek();

            foreach (var current in _slotDictionary[slot].TakeWhile(current => current.IsEqual(last)))
            {
                _lastCoinStack.Push(current);
            }

            return _lastCoinStack;
        }

        public static void Clear()
        {
            _slotDictionary.Clear();
            _lastCoinStack.Clear();
        }
    }
}