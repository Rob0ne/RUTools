using UnityEditor;
using RUT.Editor;

namespace RUT.Editor
{
    [CustomEditor(typeof(RUToolsPreferences), true), CanEditMultipleObjects]
    public class RUToolsPreferencesEditor : RUToolsEditor<RUToolsPreferences>
    {
    }
}