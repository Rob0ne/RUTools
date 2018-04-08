using UnityEditor;
using RUT.Editor;

namespace RUT.Tools.Pool
{
    [CustomEditor(typeof(MultiAssetPoolByStringIDAsset), true), CanEditMultipleObjects]
    public class MultiAssetPoolByStringIDAssetEditor : RUToolsEditor<MultiAssetPoolByStringIDAsset>
    {
    }
}