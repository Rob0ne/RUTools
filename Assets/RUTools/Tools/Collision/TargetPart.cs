using UnityEngine;

namespace RUT.Tools.Collision
{
    /// <summary>
    /// Target redirection class. Used to point toward the true target.
    /// </summary>
    public class TargetPart : MonoBehaviour, ITargetable
    {
        #region Public properties
        [SerializeField, RequireInterface(typeof(ITargetable))]
        Component target;

        public ITargetable Target
        {
            get
            {
                if(_target == null)
                    _target = target as ITargetable;

                return _target;
            }
        }
        #endregion

        #region Private properties
        private ITargetable _target = null;
        #endregion
    }

}