using System.Collections.Generic;
using CoinSortClone.Data;
using TMPro;
using UnityEngine;

namespace CoinSortClone.Pattern
{
    public class SpecialCoinPool : MonoBehaviour
    {
        [SerializeField] protected GameObject prefab;
        [SerializeField] protected Transform parentObject;
        [SerializeField] private uint amountToPool;
        [SerializeField] protected bool isAllObjectActive;

        private Stack<Coin> _poolStack = new Stack<Coin>();

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

            GameObject coinModel = newObject.GetComponentInChildren<MeshFilter>().gameObject;

            Coin coin = new Coin()
            {
                Transform = newObject.transform,
                Text = newObject.GetComponentsInChildren<TMP_Text>(),
                MeshRenderer = coinModel.GetComponent<MeshRenderer>()
            };
            _poolStack.Push(coin);
        }

        public void Push(Coin newObject)
        {
            if (parentObject.gameObject.GetInstanceID() ==
                newObject.Transform.parent.gameObject.GetInstanceID())
                _poolStack.Push(newObject);
        }

        public Coin Pop()
        {
            while (true)
            {
                if (_poolStack.Count != 0) return _poolStack.Pop();

                AddNewObject();
            }
        }
    }
}