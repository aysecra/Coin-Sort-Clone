using CoinSortClone.Interfaces;
using UnityEngine;

namespace CoinSortClone.Structs
{
    public struct DetectableObjectData
    {
        public Border Border;
        public Transform DetectableTransform;
        public IDetectable DetectableScript;
    }
}