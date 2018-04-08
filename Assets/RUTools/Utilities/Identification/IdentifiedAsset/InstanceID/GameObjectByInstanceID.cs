using UnityEngine;

namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// GameObjectByInstanceID class.
    /// </summary>
    [CreateAssetMenu(fileName = "GameObjectByInstanceID", menuName = "RUTools/Identification/Asset/GameObject by Instance ID", order = 51)]
    public class GameObjectByInstanceID : AssetByInstanceID, IIdentifiedAsset<int, GameObject>
    {
        #region Public properties
        [SerializeField] GameObject gameObject;

        public GameObject Asset
        {
            get
            {
                return gameObject;
            }
        }
        #endregion
    }
}