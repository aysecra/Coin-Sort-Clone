using System.Collections;
using CoinSortClone.Pattern;
using CoinSortClone.Interfaces;
using CoinSortClone.Manager;
using CoinSortClone.Structs;
using CoinSortClone.Test;
using UnityEngine;

namespace CoinSortClone.Component
{
    public class DetectableArea : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(IDelayedStart());
        }

        IEnumerator IDelayedStart()
        {
            yield return new WaitForSeconds(.1f);
            if (gameObject.TryGetComponent(out ObjectPool pool))
                GetDetectableObjectsFromPool(pool);
            else GetDetectableObjectsFromTransform();
        }
        
        private void GetDetectableObjectsFromPool(ObjectPool pool)
        {
            foreach (GameObject obj in pool.Objects)
            {
                GetObject(obj.transform);
            }
        }

        private void GetDetectableObjectsFromTransform()
        {
            foreach (Transform child in transform)
            {
                GetObject(child);
            }
        }

        private void GetObject(Transform objTransform)
        {
            // if (objTransform.TryGetComponent(out MeshFilter meshFilter))
            // {
            //     DetectableObjectData newObject = new DetectableObjectData()
            //     {
            //         DetectableTransform = objTransform,
            //         Mesh = meshFilter.mesh,
            //     };
            //
            //     if (objTransform.TryGetComponent(out IDetectable detectable))
            //     {
            //         newObject.DetectableScript = detectable;
            //     }
            //     else
            //     {
            //         DetectedObject newDetectedObject = new DetectedObject(newObject);
            //         newObject.DetectableScript = newDetectedObject;
            //     }
            //
            //     DetectorManager.Instance.AddDetectableObject(newObject);
            // }
        }
    }
}