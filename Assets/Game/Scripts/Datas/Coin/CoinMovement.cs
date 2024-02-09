using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoinSortClone.Component;
using CoinSortClone.Data;
using CoinSortClone.Logic;
using CoinSortClone.Manager;
using CoinSortClone.SO;
using DG.Tweening;
using UnityEngine;

namespace CoinSortClone.Component
{
    public class CoinMovement : Singleton<CoinMovement>
    {
        [SerializeField] private SlotSO slotSo;

        private Vector3 _targetPos;
        private WaitForSeconds _waitForSeconds;

        public void Move(Slot slot, Stack<Coin> coins, int direction)
        {
            StartCoroutine(IMove(slot, coins, direction));
        }

        public void Merge(Slot slot, Stack<Coin> coins)
        {
            StartCoroutine(IMerge(slot, coins));
        }

        IEnumerator IMove(Slot slot, Stack<Coin> coins, int direction)
        {
            coins = new Stack<Coin>(coins.Where(i => i != null));

            foreach (Coin coin in coins)
            {
                Quaternion startRotation = coin.Transform.rotation;
                Vector3 rot = startRotation.eulerAngles;
                rot = new Vector3(-rot.x, rot.y + direction * 180, rot.z);
                Quaternion target = Quaternion.Euler(rot);
                float rotationTime = 0;

                _targetPos = slot.StartPos;
                _targetPos.z -= slot.CoinCount * slotSo.CoinOffset;

                coin.Transform.DOJump(_targetPos, slotSo.MovementJumpPower, 1, slotSo.MovementDuration)
                    .OnUpdate((() =>
                    {
                        var factor = rotationTime / slotSo.MovementDuration;
                        coin.Transform.rotation = Quaternion.Lerp(startRotation, target, factor);
                        rotationTime += Time.deltaTime;
                    }))
                    .OnComplete((() => { coin.Transform.rotation = target; }));

                slot.IncreaseCoin(1);
                SlotController.AddCoinToSlot(slot, coin);
                yield return _waitForSeconds;
            }

            slot.ControlIsFull();
        }

        IEnumerator IMerge(Slot slot, Stack<Coin> coins)
        {
            coins = new Stack<Coin>(coins.Where(i => i != null));
            int counter = 0;
            slot.SetNotMergeable();

            foreach (var coin in coins)
            {
                if (counter < coins.Count - 1)
                {
                    _targetPos = slot.StartPos;
                    coin.Transform.DOMove(slot.StartPos,
                            slotSo.MergeDurationPerDist * Vector3.Distance(slot.StartPos, coin.Transform.position))
                        .OnComplete((() => { SlotController.RemoveLastCoin(slot); }));
                    counter++;

                    yield return _waitForSeconds;
                }
                else
                {
                    SharedLevelManager.Instance.IncreaseCoinValue(coin);
                }
            }

            slot.DecreaseCoin(coins.Count - 1);
        }

        private void OnEnable()
        {
            _waitForSeconds = new WaitForSeconds(slotSo.CoinDelay);
        }
    }
}