using UnityEngine;
using UnityEditor;
using System;
using RUT.Editor;

namespace RUT.Systems.Action
{
    [CustomEditor(typeof(TargetAction), true), CanEditMultipleObjects]
    public class TargetActionEditor : RUToolsEditor<TargetAction>
    {
        private static readonly String _idSettingsExpanded = "SettingsExpanded";

        private SerializedProperty _settingsProperty;
        private SerializedProperty _collectorProperty;

        private SerializedProperty _delayProperty;
        private SerializedProperty _lifetimeProperty;
        private SerializedProperty _repeatIntervalProperty;
        private SerializedProperty _repeatLimitProperty;
        private SerializedProperty _affectedLimitProperty;
        private SerializedProperty _minTargetsForRepeatProperty;
        private SerializedProperty _endOnNoRepeatLeftProperty;
        private SerializedProperty _waitIfNotEnoughTargetsProperty;
        private SerializedProperty _autoDisableProperty;
        private SerializedProperty _repeatTypeProperty;

        private bool _settingsExpanded = false;

        private bool _lifeTimeActive = false;
        private bool _affectedLimitActive = false;
        private bool _repeatLimitActive = false;

        public override void OnEnable()
        {
            base.OnEnable();

            _settingsProperty = serializedObject.FindProperty("settings");
            _collectorProperty = serializedObject.FindProperty("collector");

            _delayProperty = _settingsProperty.FindPropertyRelative("delay");
            _lifetimeProperty = _settingsProperty.FindPropertyRelative("lifetime");
            _repeatIntervalProperty = _settingsProperty.FindPropertyRelative("repeatInterval");
            _repeatLimitProperty = _settingsProperty.FindPropertyRelative("repeatLimit");
            _affectedLimitProperty = _settingsProperty.FindPropertyRelative("affectedLimit");
            _minTargetsForRepeatProperty = _settingsProperty.FindPropertyRelative("minTargetsForRepeat");
            _endOnNoRepeatLeftProperty = _settingsProperty.FindPropertyRelative("endOnNoRepeatLeft");
            _waitIfNotEnoughTargetsProperty = _settingsProperty.FindPropertyRelative("waitIfNotEnoughTargets");
            _autoDisableProperty = _settingsProperty.FindPropertyRelative("autoDisable");
            _repeatTypeProperty = _settingsProperty.FindPropertyRelative("repeatType");

            _lifeTimeActive = _lifetimeProperty.floatValue >= 0 ? true : false;
            _affectedLimitActive = _affectedLimitProperty.intValue >= 0 ? true : false;
            _repeatLimitActive = _repeatLimitProperty.intValue >= 0 ? true : false;
        }

        public override void SaveStates()
        {
            base.SaveStates();

            RUToolsPreferences preferences = RUToolsPreferences.GetPreferences();
            preferences.SetEditorState(target.GetType(), _idSettingsExpanded, Convert.ToInt16(_settingsExpanded));
        }

        public override void LoadStates()
        {
            base.LoadStates();

            RUToolsPreferences preferences = RUToolsPreferences.GetPreferences();
            _settingsExpanded = Convert.ToBoolean(preferences.GetEditorState(target.GetType(), _idSettingsExpanded));
        }

        public override void DrawMainSettings()
        {
            _settingsExpanded = EditorGUILayout.Foldout(_settingsExpanded, ObjectNames.NicifyVariableName(_settingsProperty.name), true);

            EditorGUI.indentLevel += 1;

            if (_settingsExpanded)
            {
                //Delay
                EditorGUILayout.PropertyField(_delayProperty);
                if (_delayProperty.floatValue < 0)
                    _delayProperty.floatValue = 0;

                //LifeTime
                _lifeTimeActive = DrawToggledField(_lifeTimeActive, () =>
                {
                    EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(_lifetimeProperty.name), "Unlimited");
                    _lifetimeProperty.floatValue = -1;
                }, () =>
                {
                    EditorGUILayout.PropertyField(_lifetimeProperty);
                    if (_lifetimeProperty.floatValue < 0)
                        _lifetimeProperty.floatValue = 0;
                });

                //Repeat
                EditorGUILayout.LabelField("Repeat");
                EditorGUI.indentLevel += 1;

                //AffectedLimit
                _affectedLimitActive = DrawToggledField(_affectedLimitActive, () =>
                {
                    EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(_affectedLimitProperty.name), "Unlimited");
                    _affectedLimitProperty.intValue = -1;
                }, () =>
                {
                    EditorGUILayout.PropertyField(_affectedLimitProperty);
                    if (_affectedLimitProperty.intValue < 0)
                        _affectedLimitProperty.intValue = 0;
                });

                //RepeatTime
                DrawButtonedField(() =>
                {
                    GenericMenu menu = new GenericMenu();

                    TargetAction.UpdateType currentType = ((TargetAction.UpdateType)_repeatTypeProperty.enumValueIndex);
                    Array enumList = Enum.GetValues(typeof(TargetAction.UpdateType));

                    foreach(TargetAction.UpdateType t in enumList)
                    {
                        TargetAction.UpdateType type = t;
                        bool active = currentType == type;

                        menu.AddItem(new GUIContent(t.ToString()), active, () =>
                        {
                            _repeatTypeProperty.enumValueIndex = (int)type;
                            serializedObject.ApplyModifiedProperties();
                        });
                    }

                    menu.ShowAsContext();
                }, () =>
                {
                    switch((TargetAction.UpdateType)_repeatTypeProperty.enumValueIndex)
                    {
                        case TargetAction.UpdateType.Update:
                            EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(_repeatIntervalProperty.name), "Update");
                            break;
                        case TargetAction.UpdateType.FixedUpdate:
                            EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(_repeatIntervalProperty.name), "FixedUpdate");
                            break;
                        default:
                            EditorGUILayout.PropertyField(_repeatIntervalProperty);
                            if (_repeatIntervalProperty.floatValue < 0)
                                _repeatIntervalProperty.floatValue = 0;
                            break;
                    }
                });

                //RepeatLimit
                _repeatLimitActive = DrawToggledField(_repeatLimitActive, () =>
                {
                    EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(_repeatLimitProperty.name), "Unlimited");
                    _repeatLimitProperty.intValue = -1;
                }, () =>
                {
                    EditorGUILayout.PropertyField(_repeatLimitProperty);
                    if (_repeatLimitProperty.intValue < 0)
                        _repeatLimitProperty.intValue = 0;

                });

                if (_repeatLimitActive)
                {
                    //EndOnNoRepeatLeft
                    EditorGUILayout.PropertyField(_endOnNoRepeatLeftProperty);
                }

                //MinTargetsForRepeat
                EditorGUILayout.PropertyField(_minTargetsForRepeatProperty);
                if (_minTargetsForRepeatProperty.intValue < 0)
                    _minTargetsForRepeatProperty.intValue = 0;
                //WaitIfNotEnoughTargets
                EditorGUILayout.PropertyField(_waitIfNotEnoughTargetsProperty);

                EditorGUI.indentLevel -= 1;

                //AutoDisable
                EditorGUILayout.PropertyField(_autoDisableProperty);
            }

            EditorGUI.indentLevel -= 1;

            //EditorGUILayout.PropertyField(_settingsProperty, true);
            EditorGUILayout.PropertyField(_collectorProperty, true);
        }

        public override void DrawAdditionalSettings()
        {
            base.DrawAdditionalSettings();
        }
    }
}