using UnityEngine;

namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// TextureByStringID class.
    /// </summary>
    [CreateAssetMenu(fileName = "TextureByStringID", menuName = "RUTools/Identification/Asset/Texture by String ID", order = 4)]
    public class TextureByStringID : AssetByStringID, IIdentifiedAsset<string, Texture>
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