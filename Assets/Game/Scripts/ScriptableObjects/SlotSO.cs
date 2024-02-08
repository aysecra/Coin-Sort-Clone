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
        [SerializeField] private float coinDelay = .5f;
        [SerializeField] private float movementDuration = .5f;
        [SerializeField] private int movementJumpPower = 3;
        [SerializeField] private float mergeDurationPerDist = .3f;
        [SerializeField] private float mergeDelay = .1f;

        public float CoinOffset => coinOffset;

        public float CoinBeginingMargin => coinBeginingMargin;

        public uint MaxCoin => maxCoin;

        public int MinSpawnCount => minSpawnCount;

        public int MaxSpawnCount => maxSpawnCount;

        public float CoinDelay => coinDelay;

        public float MovementDuration => movementDuration;

        public int MovementJumpPower => movementJumpPower;

        public float MergeDurationPerDist => mergeDurationPerDist;

        public float MergeDelay => mergeDelay;
    }
}
