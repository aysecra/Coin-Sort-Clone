using CoinSortClone.Data;
using CoinSortClone.Logic;
using CoinSortClone.Pattern;
using CoinSortClone.SO;
using UnityEngine;
using UnityEngine.Serialization;

namespace CoinSortClone.Manager
{
    public class SharedLevelManager : PersistentSingleton<SharedLevelManager>
    {
        [SerializeField] private SpecialCoinPool coinPool;
        [SerializeField] private CoinsSO coinsSo;

        public CoinSO GetRandomCoin()
        {
            return coinsSo.GetRandomCoin((int) ProgressManager.Instance.GetLastOpenedCoin());
        }

        public void IncreaseCoinValue(Coin coin)
        {
            if (coin.CoinType.Value + 1 > ProgressManager.Instance.GetLastOpenedCoin())
            {
                ProgressManager.Instance.IncreaseOpenedCoin();
            }

            CoinSO coinSo = coinsSo.GetCoinSO(coin.CoinType.Value + 1);
            coin.CoinType = coinSo;
            coin.Transform.gameObject.SetActive(true);

            foreach (var text in coin.Text)
            {
                text.text = $"{coinSo.Value}";
                text.color = coinSo.Color;
            }

            coin.MeshRenderer.material = coinSo.Material;
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
            coinsSo.EditCoinData();
        }
    }
}