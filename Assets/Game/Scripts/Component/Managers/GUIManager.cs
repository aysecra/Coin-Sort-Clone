using CoinSortClone.Logic;
using UnityEngine;

namespace CoinSortClone.Component.Manager
{
    public class GUIManager : MonoBehaviour
    {
        public void OnClickDealButton()
        {
            SlotController.Deal();
        }

        public void OnClickMergeButton()
        {
            SlotController.Merge();
        }
    }
}