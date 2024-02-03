using System.Collections.Generic;

namespace CoinSortClone.Component
{
    public static class SlotController
    {
        private static Dictionary<Slot, Stack<Coin>> _slotDictionary = new Dictionary<Slot, Stack<Coin>>();
        private static Stack<Coin> _lastCoinStack = new Stack<Coin>();

        public static void AddCoinToSlot(Slot slot, Coin coin)
        {
            _slotDictionary[slot] ??= new Stack<Coin>();
            _slotDictionary[slot].Push(coin);
        }

        public static Stack<Coin> GetLastCoinStackFromSlot(Slot slot)
        {
            _slotDictionary[slot] ??= new Stack<Coin>();
            
            _lastCoinStack.Clear();
            if (_slotDictionary[slot].Count == 0)
                return _lastCoinStack;
            
            Stack<Coin> slotStack = _slotDictionary[slot];
            Coin coin = slotStack.Pop();

            while (coin.IsEqual(slotStack.Peek()))
            {
                _lastCoinStack.Push(coin);
                coin = slotStack.Pop();
            }

            return _lastCoinStack;
        }
    }
}