using System.Collections.Generic;
using UnityEngine;

namespace CoinSortClone.Pattern
{
    public abstract class StackedGenericPool<T> : MonoBehaviour
    {
        [SerializeField] protected T prefab;
        [SerializeField] protected Transform parentObject;
        [SerializeField] private uint amountToPool;
        [SerializeField] protected bool isAllObjectActive;

        protected Stack<T> _poolStack = new Stack<T>();

        protected abstract void AddNewObject();
        public abstract void Push(T newObject);

        private void Start()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                AddNewObject();
            }
        }

        public T Pop()
        {
            while (true)
            {
                if (_poolStack.Count != 0) return _poolStack.Pop();

                AddNewObject();
            }
        }
    }
}