using System.Collections;
using System.Collections.Generic;
using CoinSortClone.Data;
using CoinSortClone.Interfaces;
using CoinSortClone.Logic;
using CoinSortClone.Manager;
using CoinSortClone.SO;
using CoinSortClone.Structs;
using DG.Tweening;
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
        [SerializeField] private Vector3 size;

        private DetectableObjectData _detectableObjectData = new DetectableObjectData();
        private Border _border;

        private Stack<Coin> _lastCoinStack;

        private Vector3 _startPos;
        private WaitForSeconds _waitForSeconds;
        private int _coinCount;
        private Vector3 _targetPos;
        private bool _isFull;

        public bool IsFull => _isFull;

        public Border Border => _border;

        public void OnDetected()
        {
            SlotSelectionController.Select(this);
        }

        public void Move(Stack<Coin> coins, int direction)
        {
            StartCoroutine(IMove(coins, direction));
        }

        IEnumerator IMove(Stack<Coin> coins, int direction)
        {
            foreach (Coin coin in coins)
            {
                Quaternion startRotation = coin.Transform.rotation;
                Vector3 rot = startRotation.eulerAngles;
                rot = new Vector3(-rot.x, rot.y + direction * 180, rot.z);
                Quaternion target = Quaternion.Euler(rot);
                float rotationTime = 0;

                _targetPos = _startPos;
                _targetPos.z -= _coinCount * slotSo.CoinOffset;

                coin.Transform.DOJump(_targetPos, slotSo.MovementJumpPower, 1, slotSo.MovementDuration)
                    .OnUpdate((() =>
                    {
                        var factor = rotationTime / slotSo.MovementDuration;
                        coin.Transform.rotation = Quaternion.Lerp(startRotation, target, factor);
                        rotationTime += Time.deltaTime;
                    }))
                    .OnComplete((() => { coin.Transform.rotation = target; }));

                SlotController.AddCoinToSlot(this, coin);
                _coinCount++;
                yield return _waitForSeconds;
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

            if (_coinCount >= slotSo.MaxCoin)
            {
                _isFull = true;
                if (SlotController.MergeControl(this)) mergeableObject.SetActive(_isFull);
            }
        }

        public void DecreaseCoin(int count)
        {
            _coinCount -= count;
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

        public void SetEnable()
        {
            _waitForSeconds = new WaitForSeconds(slotSo.CoinDelay);
            SetDetectableObjectData();
            DetectorManager.Instance.AddDetectableObject(_detectableObjectData);
            SlotController.AddSlot(this, isEnable);
            mergeableObject.SetActive(_isFull);
        }

        private void OnEnable()
        {
            SetEnable();
            enableObject.SetActive(isEnable);
            disableObject.SetActive(!isEnable);
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