using System.Collections.Generic;
using CoinSortClone.Interfaces;
using CoinSortClone.Logic;
using CoinSortClone.Manager;
using CoinSortClone.SO;
using CoinSortClone.Structs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CoinSortClone.Component
{
    public class Slot : MonoBehaviour
        , IDetectable
    {
        [SerializeField] private SlotSO slotSo;
        [SerializeField] private bool isEnable;
        [SerializeField] private Vector3 size;

        private DetectableObjectData _detectableObjectData = new DetectableObjectData();
        private Border _border;
        private Stack<Coin> _lastCoinStack;

        private void AddStartingCoin()
        {
            Vector3 targetPos = _border.Max;
            targetPos.y = 0;
            targetPos.x = transform.position.x;
            targetPos.z -= slotSo.CoinBeginingMargin;
            int amount = Random.Range(slotSo.MinSpawnCount, slotSo.MaxSpawnCount);
            CoinSO type = SharedLevelManager.Instance.GetRandomCoin();

            for (int i = 0; i < amount; i++)
            {
                Coin coin = SharedLevelManager.Instance.GetCoin(type);
                coin.Transform.transform.position = targetPos;
                targetPos.z -= slotSo.CoinOffset;
                SlotController.AddCoinToSlot(this, coin);
            }
        }

        public void OnDetected()
        {
            SlotSelectionController.Select(this);
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

        public void SetEnable()
        {
            SetDetectableObjectData();
            DetectorManager.Instance.AddDetectableObject(_detectableObjectData);
            SlotController.AddSlot(this);
        }

        private void OnEnable()
        {
            if (!isEnable) return;
            SetEnable();
            AddStartingCoin();
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