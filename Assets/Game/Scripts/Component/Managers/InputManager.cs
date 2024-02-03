using CoinSortClone.Enums;
using CoinSortClone.Interfaces;
using CoinSortClone.Logic;
using CoinSortClone.Manager;
using CoinSortClone.Structs.Event;
using UnityEngine;

namespace CoinSortClone.Component.Manager
{
    public class InputManager : Singleton<InputManager>
        , IUpdateListener
    {
        [SerializeField] private float detectInputDelay = .65f;

        private Vector3 _firstPosition;
        private Vector3 _endPosition;

        void IUpdateListener.ManagedUpdate()
        {
            if (Application.isEditor)
                DetectMouseInput(0);

            if (Application.isMobilePlatform)
                DetectTouchInput();
        }

        private void DetectTouchInput()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 pos = touch.position;

                if (touch.phase == TouchPhase.Began)
                {
                    _firstPosition = pos;
                    EventManager.TriggerEvent(new InputEvent(TouchState.Touch, _firstPosition));
                }

                else if (touch.phase == TouchPhase.Ended)
                {
                    _endPosition = pos;
                    EventManager.TriggerEvent(new InputEvent(TouchState.End, _endPosition));
                }
            }
        }


        private void DetectMouseInput(uint mouseButtonData)
        {
            if (mouseButtonData <= 3)
            {
                int mouseButton = (int) mouseButtonData;

                if (Input.GetMouseButtonDown(mouseButton))
                {
                    _firstPosition = Input.mousePosition;
                    EventManager.TriggerEvent(new InputEvent(TouchState.Touch, _firstPosition));
                }

                if (Input.GetMouseButtonUp(mouseButton))
                {
                    _endPosition = Input.mousePosition;
                    EventManager.TriggerEvent(new InputEvent(TouchState.End, _endPosition));
                }
            }
        }

        private void OnEnable()
        {
            if (!gameObject.scene.isLoaded) return;
            UpdateManager.Instance.AddListener(this);
        }

        private void OnDisable()
        {
            UpdateManager.Instance.RemoveListener(this);
        }
    }
}