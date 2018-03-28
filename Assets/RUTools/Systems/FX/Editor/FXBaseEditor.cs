using UnityEditor;
using RUT.Editor;

namespace RUT.Systems.FX
{
    [CustomEditor(typeof(FXBase), true), CanEditMultipleObjects]
    public class FXBaseEditor : RUToolsEditor<FXBase>
    {
    }
}