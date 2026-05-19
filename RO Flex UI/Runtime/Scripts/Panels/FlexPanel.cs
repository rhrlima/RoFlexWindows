using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Panels
{
    [ExecuteAlways]
    public class FlexPanel : MonoBehaviour
    {
        public enum Orientation { Vertical, Horizontal }
        private enum SizeMode { Fixed, Flex }

        [Serializable]
        private class Entry
        {
            public RectTransform rect;
            public SizeMode mode = SizeMode.Flex;
            public float fixedSize = 24f;
            public float proportion = 1f;
        }

        [Header("Panels")]
        [SerializeField] private List<Entry> entries = new();

        [Header("Layout settings")]
        [SerializeField] private float spacing = 0f;
        private Orientation orientation = Orientation.Vertical;

        public Orientation LayoutOrientation
        {
            get => orientation;
            set
            {
                orientation = value;
                Apply();
            }
        }

        private void OnEnable() => Apply();
        private void OnValidate() => Apply();

        private void Apply()
        {
            AutoFillEntries();

            if (gameObject.TryGetComponent<VerticalLayoutGroup>(out var vlg))
                vlg.spacing = spacing;

            if (gameObject.TryGetComponent<HorizontalLayoutGroup>(out var hlg))
                hlg.spacing = spacing;

            // Apply LayoutElement settings per entry, mapped to the main axis chosen
            foreach (var entry in entries)
            {
                if (entry == null || entry.rect == null) continue;

                var le = entry.rect.GetComponent<LayoutElement>();
                if (le == null) le = entry.rect.gameObject.AddComponent<LayoutElement>();

                if (orientation == Orientation.Vertical)
                {
                    // main axis = height
                    if (entry.mode == SizeMode.Fixed)
                    {
                        le.minHeight = entry.fixedSize;
                        le.preferredHeight = entry.fixedSize;
                        le.flexibleHeight = 0f;
                    }
                    else
                    {
                        le.minHeight = -1f;
                        le.preferredHeight = -1f;
                        le.flexibleHeight = Mathf.Max(0f, entry.proportion);
                    }

                    // keep cross axis unset so layout group can expand/squash it
                    le.minWidth = -1f;
                    le.preferredWidth = -1f;
                }
                else // Horizontal
                {
                    // main axis = width
                    if (entry.mode == SizeMode.Fixed)
                    {
                        le.minWidth = entry.fixedSize;
                        le.preferredWidth = entry.fixedSize;
                        le.flexibleWidth = 0f;
                    }
                    else
                    {
                        le.minWidth = -1f;
                        le.preferredWidth = -1f;
                        le.flexibleWidth = Mathf.Max(0f, entry.proportion);
                    }

                    // keep cross axis unset so layout group can expand/squash it
                    le.minHeight = -1f;
                    le.preferredHeight = -1f;
                }

                le.ignoreLayout = false;
            }
        }

        private void AutoFillEntries()
        {
            entries ??= new List<Entry>();
            if (entries.Count == 0)
            {
                for (var i = 0; i < transform.childCount; i++)
                {
                    var ch = transform.GetChild(i) as RectTransform;
                    if (ch != null) entries.Add(new Entry { rect = ch, mode = SizeMode.Flex, proportion = 1f });
                }
                return;
            }

            for (var i = 0; i < entries.Count; i++)
            {
                if (entries[i] == null) entries[i] = new Entry();
                if (entries[i].rect == null && i < transform.childCount)
                {
                    var ch = transform.GetChild(i) as RectTransform;
                    if (ch != null) entries[i].rect = ch;
                }
            }
        }
    }
}