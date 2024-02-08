using System.Collections.Generic;
using CoinSortClone.Data;
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

            foreach (var text in coin.Text)
            {
                text.text = $"{coinSo.Value}";
                text.color = coinSo.Color;
            }

            coin.MeshRenderer.material = coinSo.Material;
            return coin;
        }

        public void GetBackCoin(Coin coin)
        {
            coinPool.Push(coin);
        }

        private void OnEnable()
        {
            EventManager.Clear();
            SlotController.Clear();
            SlotSelectionController.Clear();
        }
    }
}