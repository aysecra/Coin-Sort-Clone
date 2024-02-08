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

        public static void Select(Slot slot)
        {
            Stack<Coin> coinStack = SlotController.GetLastCoinStackFromSlot(slot);

            if (!_isSelected)
            {
                _selectedSlot = slot;
                foreach (Coin coin in coinStack)
                {
                    coin.Transform.position += Vector3.up * .5f;
                    _isSelected = true;
                }
            }
            else
            {
                Stack<Coin> selectedStack = SlotController.GetLastCoinStackFromSlot(slot);

                if (_selectedSlot.GetInstanceID() != slot.GetInstanceID())
                    SlotController.Move(_selectedSlot, slot);

                else
                    foreach (Coin coin in selectedStack)
                    {
                        _slotPosition = coin.Transform.transform.position;
                        _slotPosition.y = 0;
                        coin.Transform.position = _slotPosition;
                    }

                _isSelected = false;
            }
        }

        public static void Clear()
        {
            _isSelected = false;
            _selectedSlot = _nullSlot;
        }
    }
}