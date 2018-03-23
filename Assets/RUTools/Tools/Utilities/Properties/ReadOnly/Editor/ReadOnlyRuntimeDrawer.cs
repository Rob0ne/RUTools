using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyRuntimeAttribute))]
public class ReadOnlyRuntimeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property,GUIContent label)
    {
        if (Application.isPlaying)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField (position, property, label, true);
            GUI.enabled = true;
        }
        else
            EditorGUI.PropertyField (position, property, label, true);
    }
}
