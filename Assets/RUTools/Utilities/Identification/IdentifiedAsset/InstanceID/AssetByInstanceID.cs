using UnityEngine;

namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// AssetByStringID class.
    /// </summary>
    public abstract class AssetByInstanceID : ScriptableObject
    {
        #region Public properties
        [SerializeField] InstanceID objectID;

        public int ID
        { get { return objectID.ID; } }
        #endregion
    }
}