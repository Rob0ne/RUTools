using UnityEngine;

namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// TextureByStringID class.
    /// </summary>
    [CreateAssetMenu(fileName = "TextureByStringID", menuName = "RUTools/Identification/Asset/Texture by String ID", order = 2)]
    public class TextureByStringID : AssetByStringID, IIdentifiedAsset<Texture, string>
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