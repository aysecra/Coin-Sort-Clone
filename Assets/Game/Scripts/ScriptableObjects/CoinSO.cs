using UnityEngine;

namespace CoinSortClone.SO
{
    [CreateAssetMenu(fileName = "Coin Item")]
    public class CoinSO : ScriptableObject
    {
        [SerializeField] private Material material;
        [SerializeField] private Color color;
        [SerializeField] private int value;

        public int Value => value;

        public Material Material => material;

        public Color Color => color;

        public bool IsEqual(CoinSO coinSo)
        {
            return value == coinSo.Value;
        }
    }
}