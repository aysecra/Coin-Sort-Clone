using UnityEngine;

namespace CoinSortClone.SO
{
    [CreateAssetMenu(fileName = "Slot Item")]
    public class SlotSO : ScriptableObject
    {
        [SerializeField] private float coinOffset;
        [SerializeField] private float coinBeginingMargin;
        [SerializeField] private uint maxCoin;
        [SerializeField] private int minSpawnCount  = 2;
        [SerializeField] private int maxSpawnCount  = 4;

        public float CoinOffset => coinOffset;

        public float CoinBeginingMargin => coinBeginingMargin;

        public uint MaxCoin => maxCoin;

        public int MinSpawnCount => minSpawnCount;

        public int MaxSpawnCount => maxSpawnCount;
    }
}
