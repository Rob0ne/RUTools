using UnityEngine;

namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// TextByStringID class.
    /// </summary>
    [CreateAssetMenu(fileName = "TextByInstanceID", menuName = "RUTools/Identification/Asset/Text by Instance ID", order = 50)]
    public class TextByInstanceID : AssetByInstanceID, IIdentifiedAsset<string, int>
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