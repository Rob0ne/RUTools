using UnityEngine;
using RUT.Utilities;
using RUT.Utilities.Identification.Asset;

namespace RUT.Tools.Pool
{
    /// <summary>
    /// Pool class used to store assets. Objects can be taken from the pool, if all are used, new
    /// ones will be instantiated. Taken objects need to be put back manually in the pool whenever
    /// they are not used anymore.
    /// </summary>
    [System.Serializable]
    public class AssetPool : ObjectPool<UnityObjectByStringID>
    {
        #region Public properties
        [Space(5)]
        [SerializeField]
        protected UnityObjectByStringID item;
        public int start;
        public int add;

        public override IPoolable Item
        { get { return GetPoolableComponent(item.Asset); } }
        public UnityObjectByStringID ItemContainer
        { get { return item; } }
        #endregion

        #region API
        public override void Initialise()
        {
            if (item == null || item.Asset == null)
            {
                ULog.LogDebug(GetType().Name + ": pool initialisation impossible, item is null.", ULog.Type.Warning);
                return;
            }

            if (GetPoolableComponent(item.Asset) == null)
            {
                ULog.LogDebug(GetType().Name + ": pool initialisation impossible, item is not poolable.", ULog.Type.Warning);
                return;
            }

            base.Initialise();
        }
        #endregion

        #region Private methods
        protected override bool ExtendList(bool initialisation = false)
        {
            int count = initialisation ? start : add;

            if (count <= 0)
            {
                if (!initialisation)
                    ULog.LogDebug(GetType().Name + ": pool extension impossible, tried to add " + count + " items.", ULog.Type.Warning);
                return false;
            }

            for (int i = 0; i < count; ++i)
            {
                Object obj = Object.Instantiate<Object>(item.Asset);
                IPoolable newInstance = GetPoolableComponent(obj);

                newInstance.LinkToPool(this);

                Component component = newInstance as Component;

                if (component != null)
                {
                    component.transform.SetParent(parent);
                    component.gameObject.SetActive(false);
                }

                _freeStack.Push(newInstance);
            }

            if (!initialisation)
                ULog.LogDebug(GetType().Name + ": pool extented by " + count + " \"" + item.Asset.name + "\" items.", ULog.Type.Log);

            return true;
        }

        private IPoolable GetPoolableComponent(Object obj)
        {
            if (obj as GameObject != null)
            {
                return (obj as GameObject).GetComponent<IPoolable>();
            }
            else if (obj as Component != null || obj as ScriptableObject != null)
            {
                return obj as IPoolable;
            }
            return null;
        }
        #endregion
    }
}