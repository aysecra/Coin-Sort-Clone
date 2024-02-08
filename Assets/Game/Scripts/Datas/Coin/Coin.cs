using CoinSortClone.SO;
using TMPro;
using UnityEngine;

namespace CoinSortClone.Data
{
    public class Coin
    {
        public CoinSO CoinType;
        public Transform Transform;
        public TMP_Text[] Text;
        public MeshRenderer MeshRenderer;

        public bool IsEqual(Coin coin)
        {
            if (coin == null) return false;
            return CoinType.IsEqual(coin.CoinType);
        }

        public CoinData GetSavableCoinData()
        {
            // todo: pozisyon datasi slot için kaydedilecek
            // todo: hangi slotun border'ı icerisinde ise o slot'a eklenecek coin

            return new CoinData()
            {
                Position = Transform.position,
                Value = CoinType.Value
            };
        }
    }
}