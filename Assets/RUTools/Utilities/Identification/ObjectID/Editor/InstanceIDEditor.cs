using UnityEditor;
using RUT.Editor;

namespace RUT.Utilities.Identification
{
    [CustomEditor(typeof(InstanceID), true), CanEditMultipleObjects]
    public class InstanceIDEditor : RUToolsEditor<InstanceID>
    {
    }
}