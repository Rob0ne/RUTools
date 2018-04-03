using UnityEngine;

namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// AudioByStringID class.
    /// </summary>
    [CreateAssetMenu(fileName = "AudioByStringID", menuName = "RUTools/Identification/Asset/Audio by String ID", order = 1)]
    public class AudioByStringID : AssetByStringID, IIdentifiedAsset<AudioClip, string>
    {
        #region Public properties
        [SerializeField] AudioClip clip;

        public AudioClip Asset
        {
            get
            {
                return clip;
            }
        }
        #endregion
    }
}