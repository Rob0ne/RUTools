using UnityEngine;

namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// AudioByStringID class.
    /// </summary>
    [CreateAssetMenu(fileName = "AudioByInstanceID", menuName = "RUTools/Identification/Asset/Audio by Instance ID", order = 51)]
    public class AudioByInstanceID : AssetByInstanceID, IIdentifiedAsset<AudioClip, int>
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