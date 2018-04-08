using UnityEditor;
using RUT.Editor;

namespace RUT.Tools.Pool
{
    [CustomEditor(typeof(MultiObjectPoolByStringIDAsset), true), CanEditMultipleObjects]
    public class MultiObjectPoolByStringIDAssetEditor : RUToolsEditor<MultiObjectPoolByStringIDAsset>
    {
    }
}