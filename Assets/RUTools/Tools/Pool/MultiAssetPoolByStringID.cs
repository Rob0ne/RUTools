using UnityEngine;

namespace RUT.Tools.Pool
{
    /// <summary>
    /// MultiAssetPoolByStringID class.
    /// </summary>
    [System.Serializable]
    public class MultiAssetPoolByStringID :
        MultiObjectPool<MultiAssetPoolByStringID.PoolByStringID, string, string, AssetPool>,
        IMultiObjectPool<string>
    {
        #region SubType
        [System.Serializable]
        public class PoolByStringID : PoolByID<string, string, AssetPool>
        {
            [SerializeField] AssetPool pool;

            public override string ID
            { get { return pool.ItemContainer.ID; } }

            public override AssetPool Pool
            { get { return pool; } }
        }
        #endregion
    }
}