using UnityEngine;

namespace CoinSortClone.SO
{
    [CreateAssetMenu(fileName = "Cost Item")]
    public class CostSO : ScriptableObject
    {
        [SerializeField] private uint dealCost;
        [SerializeField] private uint slotBeginingCost;
        [SerializeField] private uint slotCostIncreasing;
        [SerializeField] private uint earning;

        public uint DealCost => dealCost;

        public uint SlotBeginingCost => slotBeginingCost;

        public uint SlotCostIncreasing => slotCostIncreasing;

        public uint Earning => earning;
    }
}