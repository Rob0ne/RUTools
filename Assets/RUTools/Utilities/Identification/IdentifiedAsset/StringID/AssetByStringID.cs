using UnityEngine;

namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// AssetByStringID class.
    /// </summary>
    public abstract class AssetByStringID : ScriptableObject
    {
        #region Public properties
        [SerializeField] string localID;
        [SerializeField] StringID objectID;
        [SerializeField] IDType idType = IDType.Local;

        public string ID
        {
            get
            {
                switch (idType)
                {
                    case IDType.Object:
                        return objectID.ID;
                    default:
                        return localID;
                }
            }
        }
        #endregion

        #region SubType
        public enum IDType : uint
        {
            Local = 0,
            Object
        }
        #endregion
    }
}