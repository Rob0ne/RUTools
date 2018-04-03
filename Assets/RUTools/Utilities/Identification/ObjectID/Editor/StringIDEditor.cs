using UnityEngine;
using UnityEditor;
using RUT.Editor;

namespace RUT.Utilities.Identification
{
    [CustomEditor(typeof(StringID), true), CanEditMultipleObjects]
    public class StringIDEditor : RUToolsEditor<StringID>
    {
        private SerializedProperty _idProperty;

        public override void OnEnable()
        {
            base.OnEnable();

            _idProperty = serializedObject.FindProperty("id");
        }

        public override void DrawMainSettings()
        {
            DrawButtonedField(() =>
            {
                GenericMenu menu = new GenericMenu();

                menu.AddItem(new GUIContent("Object name ID"), false, () =>
                {
                    _idProperty.stringValue = target.name;
                    serializedObject.ApplyModifiedProperties();
                });

                menu.ShowAsContext();
            }, () =>
            {
                EditorGUILayout.PropertyField(_idProperty);
            });
        }
    }
}