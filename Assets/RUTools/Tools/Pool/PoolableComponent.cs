using UnityEngine;
using System;

namespace RUT.Tools.Pool
{
    /// <summary>
    ///	Poolable component. Only used as a component to avoid inheritance.
    /// </summary>
    public sealed class PoolableComponent : PoolableMBObject
    {
        #region Public properties
        [SerializeField, RequireInterface(typeof(IResetable))]
        Component[] resetComponents;

        public IResetable[] ResetComponents
        {
            get
            {
                if (_resetList == null)
                    _resetList = Array.ConvertAll(resetComponents, item => item as IResetable);

                return _resetList;
            }
        }
        #endregion

        #region Private properties
        private IResetable[] _resetList = null;
        #endregion

        #region API
        /// <summary>
        /// Resets every assigned components.
        /// </summary>
        public override void ResetInstance()
        {
            base.ResetInstance();

            if (_resetList != null)
            {
                for (int i = 0; i < _resetList.Length; ++i)
                {
                    if (_resetList[i] != null)
                    {
                        _resetList[i].ResetInstance();
                    }
                }
            }
        }
        #endregion
    }
}