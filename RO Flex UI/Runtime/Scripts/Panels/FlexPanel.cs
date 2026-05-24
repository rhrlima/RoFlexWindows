using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RO_Flex_UI.Panels
{
    // [ExecuteAlways]
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
        [SerializeField, HideInInspector] private Orientation orientation = Orientation.Vertical;

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

            if (TryGetComponent<HorizontalOrVerticalLayoutGroup>(out var layoutGroup))
            {
                layoutGroup.spacing = spacing;
            }

            foreach (var entry in entries)
            {
                if (entry == null || entry.rect == null)
                    continue;

                var layoutElement = entry.rect.GetComponent<LayoutElement>();

                if (layoutElement == null)
                    layoutElement = entry.rect.gameObject.AddComponent<LayoutElement>();

                ResetLayoutElement(layoutElement);

                if (orientation == Orientation.Vertical)
                {
                    ApplyVertical(entry, layoutElement);
                }
                else
                {
                    ApplyHorizontal(entry, layoutElement);
                }
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

        private static void ResetLayoutElement(LayoutElement layoutElement)
        {
            layoutElement.minWidth = -1f;
            layoutElement.preferredWidth = -1f;
            layoutElement.flexibleWidth = -1f;

            layoutElement.minHeight = -1f;
            layoutElement.preferredHeight = -1f;
            layoutElement.flexibleHeight = -1f;

            layoutElement.ignoreLayout = false;
        }

        private static void ApplyVertical(Entry entry, LayoutElement layoutElement)
        {
            if (entry.mode == SizeMode.Fixed)
            {
                layoutElement.minHeight = entry.fixedSize;
                layoutElement.preferredHeight = entry.fixedSize;
                layoutElement.flexibleHeight = 0f;
            }
            else
            {
                layoutElement.flexibleHeight =
                    Mathf.Max(0f, entry.proportion);
            }
        }

        private static void ApplyHorizontal(Entry entry, LayoutElement layoutElement)
        {
            if (entry.mode == SizeMode.Fixed)
            {
                layoutElement.minWidth = entry.fixedSize;
                layoutElement.preferredWidth = entry.fixedSize;
                layoutElement.flexibleWidth = 0f;
            }
            else
            {
                layoutElement.flexibleWidth =
                    Mathf.Max(0f, entry.proportion);
            }
        }
    }
}