using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class FlexPanel : MonoBehaviour
{
    public enum Orientation { Vertical, Horizontal }
    public enum SizeMode { Fixed, Flex }

    [Serializable]
    public class Entry
    {
        public RectTransform rect;
        public SizeMode mode = SizeMode.Flex;
        public float fixedSize = 24f;
        public float flex = 1f;
    }

    [Header("Entries")]
    public List<Entry> entries = new();

    [Header("Layout settings (kept in sync with detected LayoutGroup)")]
    public float spacing = 0f;

    void OnEnable() => Apply();
    void OnValidate() => Apply();

    /// <summary>
    /// Main entry. Call to apply LayoutElement values to children based on detected orientation.
    /// </summary>
    public void Apply()
    {
        AutoFillEntries();

        // Find existing layout groups
        var vlg = GetComponent<VerticalLayoutGroup>();
        var hlg = GetComponent<HorizontalLayoutGroup>();
        var orientationToUse = Orientation.Vertical;

        if (vlg == null && hlg == null)
        {
            // No layout groups found; nothing to sync against
            Debug.LogError($"No VerticalLayoutGroup or HorizontalLayoutGroup found on '{name}'. Cannot apply FlexPanel settings.");
            return;
        }

        // Sync spacing and common recommended settings depending on orientation
        if (vlg != null)
        {
            orientationToUse = Orientation.Vertical;
            vlg.spacing = spacing;
            vlg.childControlHeight = true;
            vlg.childForceExpandHeight = false;
            vlg.childControlWidth = true;
            vlg.childForceExpandWidth = true;
        }
        else if (hlg != null)
        {
            orientationToUse = Orientation.Horizontal;
            hlg.spacing = spacing;
            hlg.childControlWidth = true;
            hlg.childForceExpandWidth = false;
            hlg.childControlHeight = true;
            hlg.childForceExpandHeight = true;
        }
        
        // Apply LayoutElement settings per entry, mapped to the main axis chosen
        foreach (var e in entries)
        {
            if (e == null || e.rect == null) continue;

            var le = e.rect.GetComponent<LayoutElement>();
            if (le == null) le = e.rect.gameObject.AddComponent<LayoutElement>();

            if (orientationToUse == Orientation.Vertical)
            {
                // main axis = height
                if (e.mode == SizeMode.Fixed)
                {
                    le.minHeight = e.fixedSize;
                    le.preferredHeight = e.fixedSize;
                    le.flexibleHeight = 0f;
                }
                else
                {
                    le.minHeight = -1f;
                    le.preferredHeight = -1f;
                    le.flexibleHeight = Mathf.Max(0f, e.flex);
                }

                // keep cross axis unset so layout group can expand/squash it
                le.minWidth = -1f;
                le.preferredWidth = -1f;
            }
            else // Horizontal
            {
                // main axis = width
                if (e.mode == SizeMode.Fixed)
                {
                    le.minWidth = e.fixedSize;
                    le.preferredWidth = e.fixedSize;
                    le.flexibleWidth = 0f;
                }
                else
                {
                    le.minWidth = -1f;
                    le.preferredWidth = -1f;
                    le.flexibleWidth = Mathf.Max(0f, e.flex);
                }

                // keep cross axis unset so layout group can expand/squash it
                le.minHeight = -1f;
                le.preferredHeight = -1f;
            }

            le.ignoreLayout = false;
        }

#if UNITY_EDITOR
        if (!Application.isPlaying) EditorUtility.SetDirty(this);
#endif
    }

    void AutoFillEntries()
    {
        entries ??= new List<Entry>();
        if (entries.Count == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var ch = transform.GetChild(i) as RectTransform;
                if (ch != null) entries.Add(new Entry { rect = ch, mode = SizeMode.Flex, flex = 1f });
            }
            return;
        }

        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i] == null) entries[i] = new Entry();
            if (entries[i].rect == null && i < transform.childCount)
            {
                var ch = transform.GetChild(i) as RectTransform;
                if (ch != null) entries[i].rect = ch;
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Apply LayoutElement Settings")]
    private void EditorApply() => Apply();
#endif
}
