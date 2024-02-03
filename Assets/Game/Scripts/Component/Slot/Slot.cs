using System;
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
        [SerializeField] private float coinOffset;
        [SerializeField] private float coinBeginingMargin;
        [SerializeField] private uint maxCoin;

        private DetectableObjectData _detectableObjectData = new DetectableObjectData();
        private Border _border;
        private Stack<Coin> _lastCoinStack;

        private void GetCoin()
        {
            Vector3 targetPos = _border.Max;
            targetPos.y = 0;
            targetPos.x = transform.position.x;
            targetPos.z -= coinBeginingMargin;

            for (int i = 0; i < maxCoin; i++)
            {
                GameObject coin = SharedLevelManager.Instance.GetCoin();
                coin.transform.position = targetPos;
                targetPos.z -= coinOffset;
            }
        }

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

        // public void SetEnable()
        // {
        //     SetDetectableObjectData();
        //     DetectorManager.Instance.AddDetectableObject(_detectableObjectData);
        // }

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