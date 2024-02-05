using System;
using System.Collections.Generic;
using CoinSortClone.Component;
using CoinSortClone.Logic;
using CoinSortClone.Pattern;
using CoinSortClone.SO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CoinSortClone.Manager
{
    public class SharedLevelManager : PersistentSingleton<SharedLevelManager>
    {
        [SerializeField] private SpecialCoinPool coinPool;
        [SerializeField] private List<CoinSO> _coinTypeList = new List<CoinSO>();

        private object _nullObject = null;

        protected override void Awake()
        {
            base.Awake();
            EventManager.Clear();
            SlotController.Clear();
            SlotSelectionController.Clear();
        }

        public CoinSO GetRandomCoin()
        {
            if (_coinTypeList.Count == 0) return (CoinSO) _nullObject;
            int rndIndex = Random.Range(0, _coinTypeList.Count);
            return _coinTypeList[rndIndex];
        }

        public Coin GetCoin(CoinSO coinSo)
        {
            Coin coin = coinPool.Pop();
            coin.CoinType = coinSo;
            coin.Transform.gameObject.SetActive(true);

            coin.Text.text = $"{coinSo.Value}";
            coin.Text.color = coinSo.Color;

            coin.MeshRenderer.material = coinSo.Material;
            return coin;
        }
    }
}