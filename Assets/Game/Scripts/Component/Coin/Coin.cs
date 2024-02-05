using CoinSortClone.SO;
using TMPro;
using UnityEngine;

namespace CoinSortClone.Component
{
    public struct Coin
    {
        public CoinSO CoinType;
        public Transform Transform;
        public TMP_Text Text;
        public MeshRenderer MeshRenderer;

        public bool IsEqual(Coin coin)
        {
            return CoinType.IsEqual(coin.CoinType);
        }
    }
}