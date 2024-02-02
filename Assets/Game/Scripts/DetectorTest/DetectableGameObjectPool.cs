using CoinSortClone.Pattern;
using UnityEngine;

namespace CoinSortClone.Test
{
    public class DetectableGameObjectPool : GameObjectPool
    {
        [SerializeField] private Vector3 objectPositionConstant;

        protected override void AddNewObject()
        {
            GameObject newObject = Instantiate(childObject, parentObject);
            newObject.name = $"{childObject.name}-{_objects.Count}";
            newObject.SetActive(isAllObjectActive);
            newObject.transform.position =
                transform.position + new Vector3(_objects.Count * objectPositionConstant.x,
                    _objects.Count * objectPositionConstant.y, _objects.Count * objectPositionConstant.z);
            _objects.Add(newObject);
        }
    }
}