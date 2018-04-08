using UnityEngine;

namespace RUT.Tools.Pool
{
    /// <summary>
    /// MultiAssetPoolByStringIDAsset class.
    /// </summary>
    [CreateAssetMenu(fileName = "MultiAssetPoolByStringID", menuName = "RUTools/Pool/MultiAssetPool by String ID", order = 1)]
    public class MultiAssetPoolByStringIDAsset : ScriptableObject, IMultiObjectPool<string>
    {
        #region Public properties
        [SerializeField] MultiAssetPoolByStringID multiAssetPool = new MultiAssetPoolByStringID();

        public int TotalItems
        { get { return multiAssetPool.TotalItems; } }
        public int FreeItems
        { get { return multiAssetPool.FreeItems; } }
        public int UsedItems
        { get { return multiAssetPool.UsedItems; } }
        #endregion

        #region API
        public void Initialise()
        {
            multiAssetPool.Initialise();
        }
        public void Clear()
        {
            multiAssetPool.Clear();
        }
        public void DisposeAll()
        {
            multiAssetPool.DisposeAll();
        }

        public IPoolable Take(string id)
        {
            return multiAssetPool.Take(id);
        }
        public bool Recover(IPoolable instance)
        {
            return multiAssetPool.Recover(instance);
        }
        #endregion
    }
}