using System;
using System.Collections.Generic;
using CoinSortClone.Pattern;
using CoinSortClone.SO;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CoinSortClone.Manager
{
    public class SharedLevelManager : PersistentSingleton<SharedLevelManager>
    {
        [SerializeField] private StackedGameObjectPool coinPool;
        [SerializeField] private List<CoinSO> _coinTypeList = new List<CoinSO>();

        private object _nullObject = null;

        private void Start()
        {
        }

        public CoinSO GetRandomCoin()
        {
            if (_coinTypeList.Count == 0) return (CoinSO) _nullObject;
            int rndIndex = Random.Range(0, _coinTypeList.Count);
            return _coinTypeList[rndIndex];
        }

        public GameObject GetCoin()
        {
            CoinSO coinSo = GetRandomCoin();
            GameObject coinObject = coinPool.Pop();
            coinObject.SetActive(true);

            TMP_Text text = coinObject.GetComponentInChildren<TMP_Text>();
            text.text = $"{coinSo.Value}";
            text.color = coinSo.Color;

            GameObject coinModel = coinObject.GetComponentInChildren<MeshFilter>().gameObject;
            if (coinModel.TryGetComponent(out MeshRenderer meshRenderer)) meshRenderer.material = coinSo.Material;
            return coinObject;
        }
    }
}