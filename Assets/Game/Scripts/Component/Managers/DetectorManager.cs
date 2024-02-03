using System.Collections.Generic;
using CoinSortClone.Component;
using CoinSortClone.Interface;
using CoinSortClone.Logic;
using CoinSortClone.Structs;
using CoinSortClone.Structs.Event;
using CoinSortClone.Enums;
using UnityEngine;

namespace CoinSortClone.Manager
{
    public class DetectorManager : Singleton<DetectorManager>
        , EventListener<InputEvent>
    {
        private HashSet<DetectableObjectData> _detectableObjects = new HashSet<DetectableObjectData>();

        private void DetectObject(Vector3 position)
        {
            if (_detectableObjects.Count > 0)
            {
                foreach (var detectableObject in _detectableObjects)
                {
                    ScreenToWorldPointData screenToWorldPointData =
                        CameraController.Instance.ScreenToWorldPoint(position);

                    if (ObjectDetector.CalculateIsHitToObject(detectableObject, screenToWorldPointData))
                        detectableObject.DetectableScript.OnDetected();
                }
            }
        }

        public void AddDetectableObject(DetectableObjectData detectableObject)
        {
            _detectableObjects.Add(detectableObject);
        }

        public void RemoveDetectableObject(DetectableObjectData detectableObject)
        {
            if (_detectableObjects.Contains(detectableObject))
            {
                _detectableObjects.Remove(detectableObject);
            }
        }

        private void OnEnable()
        {
            EventManager.EventStartListening<InputEvent>(this);
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;

            EventManager.EventStopListening<InputEvent>(this);
        }

        public void OnEventTrigger(InputEvent currentEvent)
        {
            if (currentEvent.State == TouchState.Touch)
            {
                DetectObject(currentEvent.Position);
            }
        }
    }
}