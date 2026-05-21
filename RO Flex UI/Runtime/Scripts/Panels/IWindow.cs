using UnityEngine;

namespace RO_Flex_UI.Panels
{
    public interface IWindow
    {
        public RectTransform transform { get; }
        void ToggleVisibility();
        void ShowWindow();
        void HideWindow();
    }
}