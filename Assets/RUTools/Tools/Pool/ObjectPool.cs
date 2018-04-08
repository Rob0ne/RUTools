using UnityEngine;
using System.Collections.Generic;
using RUT.Utilities;
using System.Linq;

namespace RUT.Tools.Pool
{
    /// <summary>
    /// Pool class used to store objects. Objects can be taken from the pool, if all are used, new
    /// ones will be instantiated. Taken objects need to be put back manually in the pool whenever
    /// they are not used anymore.
    /// </summary>
    [System.Serializable]
    public class ObjectPool : ObjectPool<Object>
    {
        #region Public properties
        [Space(5)]
        [SerializeField, RequireInterface(typeof(IPoolable))]
        protected Object item;
        public int start;
        public int add;

        public override IPoolable Item
        { get { return item as IPoolable; }}
        #endregion

        #region API
        public override void Initialise()
        {
            if (item == null)
            {
                ULog.LogDebug(GetType().Name + ": pool initialisation impossible, item is null.", ULog.Type.Warning);
                return;
            }
            if (item as IPoolable == null)
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
                IPoolable newInstance = Object.Instantiate<Object>(item) as IPoolable;

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
                ULog.LogDebug(GetType().Name + ": pool extented by " + count + " \"" + item.name + "\" items.", ULog.Type.Log);

            return true;
        }
        #endregion
    }

    [System.Serializable]
    public abstract class ObjectPool<T> : IObjectPool where T : class
    {
        #region Public properties
        public Transform parent;

        public bool IsInitialised
        { get{return _initialised;} }

        public abstract IPoolable Item { get; }

        public int TotalItems
        { get { return _freeStack.Count + _usedSet.Count; } }

        public int FreeItems
        { get { return _freeStack.Count; } }

        public int UsedItems
        { get { return _usedSet.Count; } }
        #endregion

        #region Private properties
        protected Stack<IPoolable> _freeStack = new Stack<IPoolable>();
        protected HashSet<IPoolable> _usedSet = new HashSet<IPoolable>();
        protected bool _initialised = false;
        #endregion

        #region API
        /// <summary>
        /// Initialises pool's list.
        /// </summary>
        public virtual void Initialise()
        {
            if (_initialised)
            {
                ULog.LogDebug(GetType().Name+": pool has already been initialized.", ULog.Type.Log);
                return;
            }

            ExtendList(true);
            _initialised = true;
        }

        /// <summary>
        /// Clears pool's lists and destroys all objects.
        /// </summary>
        public void Clear()
        {
            if (!_initialised)
                return;

            if (_usedSet.Count > 0)
            {
                foreach (IPoolable instance in _usedSet)
                {
                    Component component = instance as Component;
                    ScriptableObject scriptable = instance as ScriptableObject;

                    if (component != null)
                        Object.Destroy(component.gameObject);
                    else if (scriptable != null)
                        Object.Destroy(scriptable);
                }
            }

            while (_freeStack.Count > 0)
            {
                IPoolable instance = _freeStack.Pop();

                Component component = instance as Component;
                ScriptableObject scriptable = instance as ScriptableObject;

                if (component != null)
                    Object.Destroy(component.gameObject);
                else if (scriptable != null)
                    Object.Destroy(scriptable);
            }

            _usedSet.Clear();
            _freeStack.Clear();

            _initialised = false;
        }

        /// <summary>
        /// Recovers every used objects.
        /// </summary>
        public void DisposeAll()
        {
            if (!_initialised)
                return;

            List<IPoolable> usedList = _usedSet.ToList();

            for (int i = 0; i < usedList.Count; ++i)
                usedList[i].Dispose();
        }
        /// <summary>
        /// Takes the first free object from pool. If none are available, new ones will be
        /// instantiated.
        /// </summary>
        public IPoolable Take()
        {
            if (!_initialised)
            {
                ULog.LogDebug(GetType().Name + ": pool must be initialised.", ULog.Type.Warning);
                return null;
            }

            IPoolable instance = null;

            do
            {
                if (_freeStack.Count <= 0)
                {
                    if (!ExtendList())
                        return null;
                }

                instance = _freeStack.Pop();

            } while (instance == null);

            _usedSet.Add(instance);

            return instance;
        }

        /// <summary>
        /// Recovers a taken object in the pool. If the object is not from this pool nothing will
        /// happen.
        /// </summary>
        public bool Recover(IPoolable instance)
        {
            if (!_initialised || instance == null)
                return false;

            if(!_usedSet.Contains(instance))
                return false;

            _usedSet.Remove(instance);
            _freeStack.Push(instance);

            Component component = instance as Component;

            if(component != null)
            {
                component.transform.SetParent(parent);
                component.gameObject.SetActive(false);
            }

            return true;
        }

        /// <summary>
        /// Checks if "instance" is in this pool.
        /// </summary>
        public bool Contains(IPoolable instance, ItemState state = ItemState.Any)
        {
            if (!_initialised || instance == null)
                return false;

            if (state != ItemState.Free)
            {
                if (_usedSet.Contains(instance))
                    return true;
            }

            if (state != ItemState.Used)
            {
                if (_freeStack.Contains(instance))
                    return true;
            }

            return false;
        }
        #endregion

        #region Private methods
        protected abstract bool ExtendList(bool initialisation = false);
        #endregion

        #region SubType
        public enum ItemState
        {
            Any,
            Used,
            Free,
        }
        #endregion
    }
}