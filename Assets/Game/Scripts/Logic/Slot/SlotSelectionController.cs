using System.Collections.Generic;
using CoinSortClone.Component;
using CoinSortClone.Data;
using UnityEngine;

namespace CoinSortClone.Logic
{
    public static class SlotSelectionController
    {
        private static bool _isSelected;
        private static Slot _selectedSlot;
        private static Slot _nullSlot = null;
        private static Vector3 _slotPosition;
        private static Coin _nullCoin = null;
        private static Stack<Coin> _selectedStack = new Stack<Coin>();

        public static void Select(Slot slot)
        {
            if (!_isSelected)
            {
                _selectedStack = SlotController.GetLastCoinStackFromSlot(slot);

                _selectedSlot = slot;
                foreach (Coin coin in _selectedStack)
                {
                    coin.Transform.position += Vector3.up * 1f;
                    _isSelected = true;
                }
            }
            else
            {
                foreach (Coin coin in _selectedStack)
                {
                    _slotPosition = coin.Transform.transform.position;
                    _slotPosition.y = 0;
                    coin.Transform.position = _slotPosition;
                }

                if (_selectedSlot.GetInstanceID() != slot.GetInstanceID())
                    Move(_selectedSlot, slot);

                _isSelected = false;
                _selectedStack.Clear();
            }
        }

        private static void Move(Slot selected, Slot target)
        {
            int targetTakableCoin = target.ControlCoinCapacity();
            Coin lastCoin = SlotController.GetLastCoin(target);

            if ((!SlotController.GetLastCoin(selected).IsEqual(lastCoin) && lastCoin != _nullCoin) ||
                targetTakableCoin <= 0)
                return;

            Stack<Coin> coins = SlotController.GetLastCoinStackFromSlot(selected, targetTakableCoin);

            int direction = target.transform.position.x >= selected.transform.position.x ? 1 : -1;

            CoinMovement.Instance.Move(target, coins, direction);
            selected.DecreaseCoin(coins.Count);
            for (int i = 0; i < coins.Count; i++)
            {
                SlotController.PopFromSlot(selected);
            }
        }

        public static void Clear()
        {
            _isSelected = false;
            _selectedSlot = _nullSlot;
        }
    }
}