using UnityEngine;

namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// UnityObjectByInstanceID class.
    /// </summary>
    [CreateAssetMenu(fileName = "UnityObjectByInstanceID", menuName = "RUTools/Identification/Asset/UnityObject by Instance ID", order = 50)]
    public class UnityObjectByInstanceID : AssetByInstanceID, IIdentifiedAsset<int, Object>
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