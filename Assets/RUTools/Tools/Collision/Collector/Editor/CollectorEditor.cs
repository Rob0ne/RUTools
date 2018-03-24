﻿using UnityEditor;
using RUT.Editor;

namespace RUT.Tools.Collision.Collector
{
    [CustomEditor(typeof(Collector), true), CanEditMultipleObjects]
    public class CollectorEditor : RUToolsEditor<Collector>
    {
        public override void DrawMainSettings()
        {
            DrawPropertiesExcluding(serializedObject, _scriptFieldName);
        }
    }
}