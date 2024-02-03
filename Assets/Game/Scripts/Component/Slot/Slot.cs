using System.Collections.Generic;
using CoinSortClone.Interfaces;
using CoinSortClone.Manager;
using CoinSortClone.Structs;
using UnityEngine;

namespace CoinSortClone.Component
{
    public class Slot : MonoBehaviour
        , IDetectable
    {
        [SerializeField] private Vector3 size;

        private DetectableObjectData _detectableObjectData = new DetectableObjectData();
        private Border _border;
        private Stack<Coin> _lastCoinStack;

        public void OnDetected()
        {
            _lastCoinStack = SlotController.GetLastCoinStackFromSlot(this);
        }

        private void SetDetectableObjectData()
        {
            _detectableObjectData.DetectableTransform = transform;

            // objects border data
            Vector3 position = transform.position;
            _border.Min = position - size * .5f;
            _border.Max = position + size * .5f;
            _detectableObjectData.Border = _border;

            _detectableObjectData.DetectableScript = this;
        }

        private void OnEnable()
        {
            SetDetectableObjectData();
            DetectorManager.Instance.AddDetectableObject(_detectableObjectData);
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            DetectorManager.Instance.RemoveDetectableObject(_detectableObjectData);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 center = transform.position + Vector3.up * size.y * .5f;
            Gizmos.DrawWireCube(center, size);
        }
#endif
    }
}