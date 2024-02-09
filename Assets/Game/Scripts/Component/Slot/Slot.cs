using System.Collections.Generic;
using CoinSortClone.Component.Manager;
using CoinSortClone.Data;
using CoinSortClone.Interfaces;
using CoinSortClone.Logic;
using CoinSortClone.Manager;
using CoinSortClone.SO;
using CoinSortClone.Structs;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CoinSortClone.Component
{
    public class Slot : MonoBehaviour
        , IDetectable
    {
        [SerializeField] private SlotSO slotSo;
        [SerializeField] private bool isEnable;
        [SerializeField] private GameObject disableObject;
        [SerializeField] private GameObject enableObject;
        [SerializeField] private GameObject mergeableObject;
        [SerializeField] private TMP_Text disableSlotText;
        [SerializeField] private Vector3 size;

        private DetectableObjectData _detectableObjectData = new DetectableObjectData();
        private Border _border;

        private Stack<Coin> _lastCoinStack;

        private Vector3 _startPos;
        private int _coinCount;
        private Vector3 _targetPos;
        private bool _isFull;
        private bool _isMergaeble;
        private int _price;

        public bool IsMergeable => _isMergaeble;

        public Border Border => _border;

        public Vector3 StartPos => _startPos;

        public int CoinCount => _coinCount;

        public bool IsEnable => isEnable;

        public void OnDetected()
        {
            if (isEnable)
                SlotSelectionController.Select(this);
            else if(ProgressManager.Instance.GetGold() >= _price)
            {
                SetEnable(true);
                SlotController.IncreaseSlotIndex();
                ProgressManager.Instance.DecreaseGold((uint)_price);
                GUIManager.Instance.UpdateGoldText();
                
            }
        }

        public void AddCoin()
        {
            int amount = Random.Range(slotSo.MinSpawnCount, slotSo.MaxSpawnCount);
            amount = (int) (_coinCount + amount <= slotSo.MaxCoin ? amount : slotSo.MaxCoin - _coinCount);

            if (amount > 0)
            {
                CoinSO type = SharedLevelManager.Instance.GetRandomCoin();

                for (int i = 0; i < amount; i++)
                {
                    Coin coin = SharedLevelManager.Instance.GetCoin(type);

                    _targetPos = _startPos;
                    _targetPos.z -= _coinCount * slotSo.CoinOffset;

                    coin.Transform.transform.position = _targetPos;
                    SlotController.AddCoinToSlot(this, coin);
                    _coinCount++;
                }
            }

            ControlIsFull();
        }

        public void SetNotMergeable()
        {
            _isFull = false;
            _isMergaeble = _isFull;
            mergeableObject.SetActive(_isFull);
        }

        public void ControlIsFull()
        {
            if (_coinCount < slotSo.MaxCoin) return;

            _isFull = true;
            if (SlotController.MergeControl(this))
            {
                _isMergaeble = _isFull;
                mergeableObject.SetActive(_isFull);
            }
        }

        public void ShowSlotPrice(int price)
        {
            _price = price;
            disableSlotText.text = $"{price}";
            disableSlotText.gameObject.SetActive(true);
        }

        public void DecreaseCoin(int count)
        {
            _coinCount -= count;
        }

        public void IncreaseCoin(int count)
        {
            _coinCount += count;
        }

        public int ControlCoinCapacity()
        {
            ControlIsFull();
            return (int) (slotSo.MaxCoin - _coinCount);
        }

        private void SetBeginingPosition()
        {
            _startPos = _border.Max;
            _startPos.y = 0;
            _startPos.x = transform.position.x;
            _startPos.z -= slotSo.CoinBeginingMargin;
        }

        private void SetDetectableObjectData()
        {
            _detectableObjectData.DetectableTransform = transform;

            // objects border data
            Vector3 position = transform.position;
            _border.Center = position;
            _border.Min = position - size * .5f;
            _border.Max = position + size * .5f;
            _detectableObjectData.Border = _border;

            _detectableObjectData.DetectableScript = this;
        }

        private void SetEnable(bool condition)
        {
            isEnable = condition;
            SetDetectableObjectData();
            DetectorManager.Instance.AddDetectableObject(_detectableObjectData);
            SlotController.AddSlot(this, condition);
            mergeableObject.SetActive(_isFull);
            enableObject.SetActive(isEnable);
            disableObject.SetActive(!isEnable);
        }

        private void OnEnable()
        {
            SetEnable(isEnable);
            SetBeginingPosition();
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