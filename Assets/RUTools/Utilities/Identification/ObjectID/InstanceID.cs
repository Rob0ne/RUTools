using UnityEngine;

namespace RUT.Utilities.Identification
{
    /// <summary>
    /// InstanceID class.
    /// </summary>
    [CreateAssetMenu(fileName = "InstanceID", menuName = "RUTools/Identification/Object ID/Instance ID", order = 1)]
    public class InstanceID : ScriptableObject, IObjectID<int>
    {
        #region Public properties
        public int ID
        { get { return GetInstanceID(); } }
        #endregion
    }
}