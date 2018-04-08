using UnityEngine;

namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// TextureByStringID class.
    /// </summary>
    [CreateAssetMenu(fileName = "TextureByInstanceID", menuName = "RUTools/Identification/Asset/Texture by Instance ID", order = 54)]
    public class TextureByInstanceID : AssetByInstanceID, IIdentifiedAsset<int, Texture>
    {
        #region Public properties
        [SerializeField] Texture texture;

        public Texture Asset
        {
            get
            {
                return texture;
            }
        }
        #endregion
    }
}