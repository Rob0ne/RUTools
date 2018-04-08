using UnityEngine;

namespace RUT.Utilities.Identification.Asset
{
    /// <summary>
    /// GameObjectByStringID class.
    /// </summary>
    [CreateAssetMenu(fileName = "GameObjectByStringID", menuName = "RUTools/Identification/Asset/GameObject by String ID", order = 1)]
    public class GameObjectByStringID : AssetByStringID, IIdentifiedAsset<string, GameObject>
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