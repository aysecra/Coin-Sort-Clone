using UnityEngine;

namespace CoinSortClone.Pattern
{
    public class StackedGameObjectPool : StackedGenericPool<GameObject>
    {
        protected override void AddNewObject()
        {
            GameObject newObject = Instantiate(prefab, parentObject);
            newObject.SetActive(isAllObjectActive);
            _poolStack.Push(newObject);
        }

        public override void Push(GameObject newObject)
        {
            if (parentObject.gameObject.GetInstanceID() == newObject.transform.parent.gameObject.GetInstanceID())
                _poolStack.Push(newObject);
        }
    }
}