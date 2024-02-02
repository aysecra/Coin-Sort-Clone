using CoinSortClone.Interfaces;
using CoinSortClone.Manager;
using CoinSortClone.Structs;
using UnityEngine;

namespace CoinSortClone.Component
{
    public class DetectableObject : MonoBehaviour
        , IDetectable
    {
        [SerializeField] private MeshFilter meshFilter;

        private DetectableObjectData _currDetectableObjectData;

        public void OnDetected()
        {
            print(gameObject.name + " is detected");
        }

        private void OnEnable()
        {
            if (meshFilter != null)
            {
                _currDetectableObjectData = new DetectableObjectData()
                {
                    Mesh = meshFilter.mesh,
                    DetectableTransform = transform,
                    DetectableScript = this
                };
                
                DetectorManager.Instance.AddDetectableObject(_currDetectableObjectData);
            }
        }
        
        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;

            if (meshFilter != null)
            {
                DetectorManager.Instance.RemoveDetectableObject(_currDetectableObjectData);
            }
        }
    }
}