using UnityEditor;
using UnityEngine;
using RUT.Editor;

namespace RUT.Tools.Localization
{
    [CustomEditor(typeof(LocalizationControllerByStringIDAsset), true), CanEditMultipleObjects]
    public class LocalizationControllerByStringIDAssetEditor : RUToolsEditor<LocalizationControllerByStringIDAsset>
    {
        public override void DrawAdditionalSettings()
        {
            DrawButtonedField(() =>
            {
                GenericMenu menu = new GenericMenu();

                menu.AddItem(new GUIContent("Create text file template"), false, () =>
                {
                    LocalizationControllerByStringIDAsset controller = target as LocalizationControllerByStringIDAsset;

                    string path = AssetDatabase.GetAssetPath(controller);
                    int assetNameLength = controller.name.Length + 6;
                    path = path.Remove(path.Length - assetNameLength) + "TextFileTemplate";

                    controller.CreateTextFileTemplate(path);
                    AssetDatabase.Refresh();
                });

                menu.ShowAsContext();
            }, () =>
            {
                EditorGUILayout.LabelField("OPTIONS");
            });

            base.DrawAdditionalSettings();
        }

        public override void OnEnable()
        {
            base.OnEnable();

            _showDerivedInspector = true;
        }
    }
}