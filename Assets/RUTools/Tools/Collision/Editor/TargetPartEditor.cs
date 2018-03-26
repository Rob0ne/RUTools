using UnityEditor;
using RUT.Editor;

namespace RUT.Tools.Collision
{
    [CustomEditor(typeof(TargetPart), true), CanEditMultipleObjects]
    public class TargetPartEditor : RUToolsEditor<TargetPart>
    {
        public override void DrawMainSettings()
        {
            DrawPropertiesExcluding(serializedObject, _scriptFieldName);
        }
    }
}