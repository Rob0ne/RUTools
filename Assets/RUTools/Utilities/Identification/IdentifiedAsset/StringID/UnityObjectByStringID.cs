using UnityEngine;

namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// UnityObjectByStringID class.
    /// </summary>
    [CreateAssetMenu(fileName = "UnityObjectByStringID", menuName = "RUTools/Identification/Asset/UnityObject by String ID", order = 0)]
    public class UnityObjectByStringID : AssetByStringID, IIdentifiedAsset<string, Object>
    {
        #region Public properties
        [SerializeField] Object obj;

        public Object Asset
        {
            get
            {
                return obj;
            }
        }
        #endregion
    }
}