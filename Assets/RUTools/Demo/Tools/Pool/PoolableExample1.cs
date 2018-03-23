using UnityEngine;
using RUT.Tools.Pool;

namespace RUT.Examples.Pool
{
    /// <summary>
    /// Example of poolable object using PoolableMBObject. The object is directly poolable.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PoolableExample1 : PoolableMBObject
    {
        #region Private properties
        private Rigidbody _rigidbody;
        #endregion

        #region API
        public override void ResetInstance()
        {
            base.ResetInstance();

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
        #endregion

        #region Unity
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        #endregion
    }
}