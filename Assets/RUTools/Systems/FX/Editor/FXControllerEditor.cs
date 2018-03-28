using UnityEditor;
using RUT.Editor;

namespace RUT.Systems.FX
{
    [CustomEditor(typeof(FXController), true), CanEditMultipleObjects]
    public class FXControllerEditor : RUToolsEditor<FXController>
    {
    }
}