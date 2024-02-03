
using CoinSortClone.Structs;
using UnityEngine;

namespace CoinSortClone.Logic
{
    public static class Calculator
    {
        public static Vector3 LocalPointToWorld(Vector3 p, Transform objectTransform) =>
            objectTransform.localToWorldMatrix.MultiplyPoint3x4(p);

        public static bool IsInBorder(Vector3 target, Border border)
        {
            if (target.x > border.Max.x || target.x < border.Min.x) return false;
            if (target.z > border.Max.z || target.z < border.Min.z) return false;

            return true;
        }

        public static Vector3 PossibleHitPoint(Vector3 center, ScreenToWorldPointData ray)
        {
            float dist = Vector3.Distance(center, ray.Origin);
            return ray.Origin + ray.Direction * dist;
        }
    }
}