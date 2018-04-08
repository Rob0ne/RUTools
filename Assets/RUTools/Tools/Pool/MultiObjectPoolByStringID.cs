using UnityEngine;

namespace RUT.Tools.Pool
{
    /// <summary>
    /// MultiObjectPoolByStringID class.
    /// </summary>
    [System.Serializable]
    public class MultiObjectPoolByStringID :
        MultiObjectPool<MultiObjectPoolByStringID.PoolByStringID, string, string, ObjectPool>,
        IMultiObjectPool<string>
    {
        #region SubType
        [System.Serializable]
        public class PoolByStringID : PoolByID<string, string, ObjectPool>
        {
            [ReadOnlyRuntime]
            [SerializeField] string id;
            [SerializeField] ObjectPool pool;

            public override string ID
            { get { return id; } }

            public override ObjectPool Pool
            { get { return pool; } }
        }
        #endregion
    }
}