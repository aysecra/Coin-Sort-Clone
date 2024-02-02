using CoinSortClone.Interfaces;
using UnityEngine;

namespace CoinSortClone.Structs
{
    public struct DetectableObjectData
    {
        public Mesh Mesh;
        public Transform DetectableTransform;
        public IDetectable DetectableScript;
    }
}