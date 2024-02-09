using System;
using System.Collections;
using CoinSortClone.Logic;
using UnityEngine;

namespace CoinSortClone.Component.Manager
{
    public class GUIManager : Singleton<GUIManager>
    {
        [SerializeField] private TMPro.TextMeshProUGUI goldText;

        IEnumerator Start()
        {
            yield return new WaitForSeconds(.05f);
            UpdateGoldText();
        }

        public void UpdateGoldText()
        {
            goldText.text = $"{ProgressManager.Instance.GetGold()}";
        }

        public void OnClickDealButton()
        {
            ProgressManager.Instance.DecreaseGold();
            UpdateGoldText();
            SlotController.Deal();
        }

        public void OnClickMergeButton()
        {
            SlotController.Merge();
        }
    }
}