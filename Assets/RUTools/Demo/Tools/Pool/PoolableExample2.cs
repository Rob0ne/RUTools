using UnityEngine;
using RUT.Tools.Pool;

namespace RUT.Examples.Pool
{
    /// <summary>
    /// Example of poolable object using PoolableComponent. The PoolableComponent is the object
    /// being pooled and in the configuration of this PoolableComponent, you can add IResetable
    /// scripts that should be reset when the component is recovered. For this example,
    /// PoolableExample2 implements IDisposable and redirect the Dispose function to the
    /// PoolableComponent. That way you don't have to actually have the PoolableComponent reference
    /// to Dispose of this instance.
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(PoolableComponent))]
    public class PoolableExample2 : MonoBehaviour, IDisposable
    {
        #region Private properties
        private Rigidbody _rigidbody;
        private PoolableComponent _poolableComponent;
        #endregion

        #region API
        public void Dispose()
        {
            _poolableComponent.Dispose();
        }

        public void ResetInstance()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
        #endregion

        #region Unity
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _poolableComponent = GetComponent<PoolableComponent>();
        }
        #endregion
    }
}