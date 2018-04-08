using UnityEngine;

namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// AudioByStringID class.
    /// </summary>
    [CreateAssetMenu(fileName = "AudioByStringID", menuName = "RUTools/Identification/Asset/Audio by String ID", order = 3)]
    public class AudioByStringID : AssetByStringID, IIdentifiedAsset<string, AudioClip>
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