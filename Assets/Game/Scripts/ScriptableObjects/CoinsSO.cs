using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CoinSortClone.SO
{
    [CreateAssetMenu(fileName = "All Coin Element")]
    public class CoinsSO : ScriptableObject
    {
        [SerializeField] private List<CoinSO> _coinList = new List<CoinSO>();

        private Dictionary<int, CoinSO> _coinDict = new Dictionary<int, CoinSO>();
        private CoinSO _nullObject = null;

        private void OnValidate()
        {
            EditCoinData();
        }

        public void EditCoinData()
        {
            foreach (var coin in _coinList.Where(coin => !_coinDict.ContainsKey(coin.Value)))
            {
                _coinDict[coin.Value] = coin;
            }
        }

        public CoinSO GetCoinSO(int key)
        {
            if (_coinDict.ContainsKey(key))
            {
                return _coinDict[key];
            }

            return _nullObject;
        }

        public CoinSO GetRandomCoin(int lastOpened)
        {
            if (_coinList.Count == 0)
            {
                return _nullObject;
            }

            // int rndIndex = Random.Range(0, _coinList.Count);
            // return _coinList[rndIndex];

            try
            {
                return _coinDict.ElementAt(Random.Range(0, lastOpened)).Value;
            }
            catch (Exception e)
            {
                Debug.Log("Exception: " + e);
                return _nullObject;
            }
        }
    }
}