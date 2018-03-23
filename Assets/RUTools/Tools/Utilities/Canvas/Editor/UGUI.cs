using UnityEditor;
using UnityEngine;

namespace RUT.Tools.Utilities
{
    public class UGUI
    {
        [MenuItem("RUTools/UGUI/Anchors to Corners %PGUP")]
        static void AnchorsToCorners()
        {
            for (int i = 0; i < Selection.transforms.Length; ++i)
            {
                RectTransform t = Selection.transforms[i] as RectTransform;
                RectTransform pt = Selection.transforms[i].parent as RectTransform;

                if (t == null || pt == null)
                    return;

                Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
                                                    t.anchorMin.y + t.offsetMin.y / pt.rect.height);
                Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
                                                    t.anchorMax.y + t.offsetMax.y / pt.rect.height);

                Undo.RecordObject(t, "RectTransform changed.");

                t.anchorMin = newAnchorsMin;
                t.anchorMax = newAnchorsMax;
                t.offsetMin = t.offsetMax = Vector2.zero;
            }
        }

        [MenuItem("RUTools/UGUI/Corners to Anchors %PGDN")]
        static void CornersToAnchors()
        {
            for (int i = 0; i < Selection.transforms.Length; ++i)
            {
                RectTransform t = Selection.transforms[i] as RectTransform;

                if (t == null)
                    return;

                Undo.RecordObject(t, "RectTransform changed.");

                t.offsetMin = t.offsetMax = Vector2.zero;
            }
        }
    }
}