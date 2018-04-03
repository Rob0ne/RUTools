using UnityEngine;

namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// TextureByStringID class.
    /// </summary>
    [CreateAssetMenu(fileName = "TextureByInstanceID", menuName = "RUTools/Identification/Asset/Texture by Instance ID", order = 52)]
    public class TextureByInstanceID : AssetByInstanceID, IIdentifiedAsset<Texture, int>
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