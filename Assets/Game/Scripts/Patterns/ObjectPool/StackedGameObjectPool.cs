using System.Collections.Generic;
using UnityEngine;

namespace CoinSortClone.Pattern
{
    public class StackedGameObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform parentObject;
        [SerializeField] private uint amountToPool;
        [SerializeField] protected bool isAllObjectActive;

        private Stack<GameObject> _poolStack = new Stack<GameObject>();

        private void Start()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                AddNewObject();
            }
        }

        private void AddNewObject()
        {
            GameObject newObject = Instantiate(prefab, parentObject);
            newObject.SetActive(isAllObjectActive);
            _poolStack.Push(newObject);
        }

        public GameObject Pop()
        {
            while (true)
            {
                if (_poolStack.Count != 0) return _poolStack.Pop();

                AddNewObject();
            }
        }

        public void Push(GameObject newObject)
        {
            if (parentObject.gameObject.GetInstanceID() == newObject.transform.parent.gameObject.GetInstanceID())
                _poolStack.Push(newObject);
        }
    }
}