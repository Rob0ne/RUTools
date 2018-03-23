using UnityEngine;
using RUT.Tools.Utilities;

namespace RUT.Tools.Pool
{
    /// <summary>
    ///	Poolabe object base class.
    /// </summary>
    public abstract class PoolableMBObject : MonoBehaviour, IPoolable
{
        #region Private properties
        private ObjectPool _linkedPool;
        #endregion

        #region API
        /// <summary>
        /// Resets object's data.
        /// </summary>
        public virtual void ResetInstance()
        {
            StopAllCoroutines();
            CancelInvoke();
        }

        /// <summary>
        /// Deactivates and resets object.
        /// </summary>
        public virtual void Dispose()
        {
            bool success = true;

            if (_linkedPool != null)
                success = _linkedPool.Recover(this);
            else
                gameObject.SetActive(false);

            if (success)
            {
                ResetInstance();
            }
        }

        /// <summary>
        /// Links object to a pool.
        /// </summary>
        public void LinkToPool(ObjectPool pool)
        {
            if (_linkedPool == null)
                _linkedPool = pool;
            else
            {
                ULog.LogDebug("PoolableObject: " + gameObject.name + " has already been linked to a pool.", ULog.Type.Warning);
            }
        }
        #endregion
    }
}