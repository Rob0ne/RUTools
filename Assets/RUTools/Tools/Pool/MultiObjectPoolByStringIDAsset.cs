using UnityEngine;

namespace RUT.Tools.Pool
{
    /// <summary>
    /// MultiObjectPoolByStringID class.
    /// </summary>
    [CreateAssetMenu(fileName = "MultiObjectPoolByStringID", menuName = "RUTools/Pool/MultiObjectPool by String ID", order = 0)]
    public class MultiObjectPoolByStringIDAsset : ScriptableObject, IMultiObjectPool<string>
    {
        #region Public properties
        [SerializeField] MultiObjectPoolByStringID multiObjectPool = new MultiObjectPoolByStringID();

        public int TotalItems
        { get { return multiObjectPool.TotalItems; } }
        public int FreeItems
        { get { return multiObjectPool.FreeItems; } }
        public int UsedItems
        { get { return multiObjectPool.UsedItems; } }
        #endregion

        #region API
        public void Initialise()
        {
            multiObjectPool.Initialise();
        }
        public void Clear()
        {
            multiObjectPool.Clear();
        }
        public void DisposeAll()
        {
            multiObjectPool.DisposeAll();
        }

        public IPoolable Take(string id)
        {
            return multiObjectPool.Take(id);
        }
        public bool Recover(IPoolable instance)
        {
            return multiObjectPool.Recover(instance);
        }
        #endregion
    }
}