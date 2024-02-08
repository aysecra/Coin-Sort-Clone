using UnityEngine;

namespace CoinSortClone.SO
{
    [CreateAssetMenu(fileName = "Cost Item")]
    public class CostSO : ScriptableObject
    {
        [SerializeField] private uint dealCost;
        [SerializeField] private uint slotBeginingCost;
        [SerializeField] private uint slotCostIncreasing;
    }
}