using UnityEngine;
using UnityEditor;
using RUT.Editor;
using System;

namespace RUT.Utilities.Identification.Asset
{
    [CustomEditor(typeof(AssetByStringID), true), CanEditMultipleObjects]
    public class AssetByStringIDEditor : RUToolsEditor<AssetByStringID>
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

                AssetByStringID.IDType currentType = ((AssetByStringID.IDType)_idTypeProperty.enumValueIndex);
                Array enumList = Enum.GetValues(typeof(AssetByStringID.IDType));

                foreach (AssetByStringID.IDType type in enumList)
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
                switch ((AssetByStringID.IDType)_idTypeProperty.enumValueIndex)
                {
                    case AssetByStringID.IDType.Local:
                        EditorGUILayout.PropertyField(_localIDProperty);
                        break;
                    case AssetByStringID.IDType.Object:
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