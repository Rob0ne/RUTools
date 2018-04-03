using UnityEngine;
using UnityEditor;
using RUT.Editor;
using System;


namespace RUT.Tools.Localization
{
    [CustomEditor(typeof(LocAgentByStringID), true), CanEditMultipleObjects]
    public class LocAgentByStringIDEditor : RUToolsEditor<LocAgentByStringID>
    {
        private SerializedProperty _localIDProperty;
        private SerializedProperty _objectIDProperty;
        private SerializedProperty _idTypeProperty;

        public override void OnEnable()
        {
            base.OnEnable();

            _localIDProperty = serializedObject.FindProperty("localID");
            _objectIDProperty = serializedObject.FindProperty("objectID");

            _idTypeProperty = serializedObject.FindProperty("idType");
        }

        public override void DrawMainSettings()
        {
            DrawButtonedField(() =>
            {
                GenericMenu menu = new GenericMenu();

                LocAgentByStringID.IDType currentType = ((LocAgentByStringID.IDType)_idTypeProperty.enumValueIndex);
                Array enumList = Enum.GetValues(typeof(LocAgentByStringID.IDType));

                foreach (LocAgentByStringID.IDType type in enumList)
                {
                    bool active = currentType == type;

                    menu.AddItem(new GUIContent(type.ToString()), active, () =>
                    {
                        _idTypeProperty.enumValueIndex = (int)type;
                        serializedObject.ApplyModifiedProperties();
                    });
                }

                menu.ShowAsContext();
            }, () =>
            {
                switch ((LocAgentByStringID.IDType)_idTypeProperty.enumValueIndex)
                {
                    case LocAgentByStringID.IDType.Local:
                        EditorGUILayout.PropertyField(_localIDProperty);
                        break;
                    case LocAgentByStringID.IDType.Object:
                        EditorGUILayout.PropertyField(_objectIDProperty);
                        break;
                    default:
                        EditorGUILayout.PropertyField(_localIDProperty);
                        break;
                }
            });
        }
    }
}