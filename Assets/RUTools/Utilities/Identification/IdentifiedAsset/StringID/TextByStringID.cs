using UnityEngine;

namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// TextByStringID class.
    /// </summary>
    [CreateAssetMenu(fileName = "TextByStringID", menuName = "RUTools/Identification/Asset/Text by String ID", order = 2)]
    public class TextByStringID : AssetByStringID, IIdentifiedAsset<string, string>
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