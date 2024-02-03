using CoinSortClone.SO;
using UnityEngine;

namespace CoinSortClone.Component
{
    public struct Coin
    {
        public CoinSO CoinType;
        public GameObject GameObject;

        public bool IsEqual(Coin coin)
        {
            return CoinType.IsEqual(coin.CoinType);
        }
    }
}