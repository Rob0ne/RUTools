using UnityEngine;

namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// TextByStringID class.
    /// </summary>
    [CreateAssetMenu(fileName = "TextByInstanceID", menuName = "RUTools/Identification/Asset/Text by Instance ID", order = 52)]
    public class TextByInstanceID : AssetByInstanceID, IIdentifiedAsset<int, string>
    {
        #region Public properties
        [SerializeField] string text;

        public string Asset
        {
            get
            {
                return text;
            }
        }
        #endregion
    }
}