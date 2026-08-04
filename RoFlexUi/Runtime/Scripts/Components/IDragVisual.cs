using UnityEngine;

namespace RO_Flex_UI.Components
{
    public readonly struct DragPresentation
    {
        public DragPresentation(Sprite sprite, string amount, string text = "")
        {
            Sprite = sprite;
            Amount = amount ?? string.Empty;
            Text = text ?? string.Empty;
        }

        public Sprite Sprite { get; }
        public string Amount { get; }
        public string Text { get; }
    }

    public interface IDragVisual
    {
        RectTransform RectTransform { get; }
        bool IsVisible { get; }
        bool EnsureReferences();
        DragPresentation CapturePresentation();
        bool TryApplyPresentation(DragPresentation presentation);
        void SetActive(bool value);
        void Clear();
    }
}
