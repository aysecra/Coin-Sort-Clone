using CoinSortClone.Structs;
using UnityEngine;

namespace CoinSortClone.Logic
{
    public static class ObjectDetector
    {
        public static bool CalculateIsHitToObject(DetectableObjectData objectData,
            ScreenToWorldPointData ray)
        {
            return IsHit(objectData, ray);
        }

        private static bool IsHit(DetectableObjectData objectData, ScreenToWorldPointData ray)
        {
            Vector3 hitPoint = Calculator.PossibleHitPoint(objectData.DetectableTransform.position, ray);
            return Calculator.IsInBorder(hitPoint, objectData.Border);
        }
    }
}