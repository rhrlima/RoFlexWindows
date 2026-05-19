using UnityEngine;

namespace RO_Flex_UI.Components
{
    public interface IWindow
    {
        public RectTransform transform { get; }
        void ToggleVisibility();
        void ShowWindow();
        void HideWindow();
    }
}