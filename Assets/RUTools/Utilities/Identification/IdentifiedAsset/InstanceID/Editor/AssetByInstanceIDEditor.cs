using UnityEngine;
using UnityEditor;
using RUT.Editor;
using System;

namespace RUT.Utilities.Identification.Asset
{
    [CustomEditor(typeof(AssetByInstanceID), true), CanEditMultipleObjects]
    public class AssetByInstanceIDEditor : RUToolsEditor<AssetByInstanceID>
    {
    }
}