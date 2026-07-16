using RO_Flex_UI.Components;
using RO_Flex_UI.Windows;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Panels
{
    public class Window : MonoBehaviour, IWindow, IPointerDownHandler
    {
        public KeyCode shortcutKey = KeyCode.None;
        public WindowId windowId = WindowId.NONE;

        [Header("Window Settings")]
        [SerializeField] private bool resetToCenter;
        [SerializeField] private bool isDraggable;
        [SerializeField] private bool isResizable;
        [SerializeField] private bool keepWindowInScreen = false;
        [SerializeField] private bool returnToOrigin;

        private Draggable draggableComponent;
        private Resizable resizeComponent;

        RectTransform IWindow.transform => transform as RectTransform;

        protected virtual void Awake()
        {
            // if components are null, behavior is ignored
            draggableComponent = GetComponentInChildren<Draggable>(true);
            resizeComponent = GetComponentInChildren<Resizable>(true);

            OnValidate();
            SyncReturnToOriginFlag();

            UiManager.Instance.RegisterWindow(windowId, this, shortcutKey);
            HideWindow();
        }

        private void OnEnable()
        {
            if (resetToCenter)
                CenterWindow();
            else
                FitWindowIntoPlayArea();
        }

        private void LateUpdate()
        {
            if (keepWindowInScreen)
                FitWindowIntoPlayArea();
        }

        private void OnValidate()
        {
            ToggleDraggable();
            ToggleResisable();
            SyncReturnToOriginFlag();
        }

        private void SyncReturnToOriginFlag()
        {
            if (draggableComponent != null)
            {
                draggableComponent.ReturnToOrigin = returnToOrigin;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left
             && eventData.button != PointerEventData.InputButton.Right)
                return;

            // bring it forward
            transform.SetAsLastSibling();
        }

        public void ToggleVisibility()
        {
            if (gameObject.activeInHierarchy)
                HideWindow();
            else
                ShowWindow();
        }

        public void ToggleDraggable()
        {
            if (draggableComponent != null && isDraggable != draggableComponent.enabled)
            {
                draggableComponent.enabled = isDraggable;
            }
        }

        public void ToggleResisable()
        {
            if (resizeComponent != null)
            {
                resizeComponent.gameObject.SetActive(isResizable);
                resizeComponent.enabled = isResizable;
            }
        }

        public virtual void ShowWindow()
        {
            transform.SetAsLastSibling();

            if (!isActiveAndEnabled)
                gameObject.SetActive(true);
        }

        public void HideWindow()
        {
            gameObject.SetActive(false);
        }

        public void CenterWindow()
        {
            var rectTransform = transform as RectTransform;
            var canvas = gameObject.transform.parent as RectTransform;

            if (rectTransform == null || canvas == null)
                return;

            var canvasSize = canvas.rect.size;
            var windowSize = rectTransform.rect.size;

            // Calculate the anchor point position in anchored position space
            // Anchor center normalized to (-0.5, -0.5) to (0.5, 0.5) range
            var anchorCenterX = (rectTransform.anchorMin.x + rectTransform.anchorMax.x) * 0.5f - 0.5f;
            var anchorCenterY = (rectTransform.anchorMin.y + rectTransform.anchorMax.y) * 0.5f - 0.5f;

            // Convert anchor offset to canvas space
            var anchorOffsetX = anchorCenterX * canvasSize.x;
            var anchorOffsetY = anchorCenterY * canvasSize.y;

            // Account for pivot offset
            var pivotOffsetX = (rectTransform.pivot.x - 0.5f) * windowSize.x;
            var pivotOffsetY = (rectTransform.pivot.y - 0.5f) * windowSize.y;

            // Set anchored position to center the window
            rectTransform.anchoredPosition = new Vector2(
                pivotOffsetX - anchorOffsetX,
                pivotOffsetY - anchorOffsetY
            );
        }

        public void FitWindowIntoPlayArea()
        {
            if (!keepWindowInScreen)
                return;

            var rectTransform = transform as RectTransform;
            var canvas = gameObject.transform.parent as RectTransform;

            if (rectTransform == null || canvas == null)
                return;

            // Get window bounds in canvas space
            var windowRect = rectTransform.rect;
            var windowPos = rectTransform.anchoredPosition;
            var canvasRect = canvas.rect;

            // Calculate the visual edges of the window relative to its anchored position
            // Left/Right edges accounting for window width and pivot
            var windowLeftOffset = -rectTransform.pivot.x * windowRect.width;
            var windowRightOffset = (1f - rectTransform.pivot.x) * windowRect.width;
            var windowBottomOffset = -rectTransform.pivot.y * windowRect.height;
            var windowTopOffset = (1f - rectTransform.pivot.y) * windowRect.height;

            // Canvas bounds in anchored position space, accounting for the window's anchor
            // The window's anchor position determines the origin of its anchored position coordinate system
            var anchorMinX = rectTransform.anchorMin.x;
            var anchorMaxX = rectTransform.anchorMax.x;
            var anchorMinY = rectTransform.anchorMin.y;
            var anchorMaxY = rectTransform.anchorMax.y;

            var canvasMinX = -anchorMinX * canvasRect.width;
            var canvasMaxX = (1f - anchorMaxX) * canvasRect.width;
            var canvasMinY = -anchorMaxY * canvasRect.height;
            var canvasMaxY = (1f - anchorMinY) * canvasRect.height;

            // Clamp position so window stays within canvas
            var clampedX = Mathf.Clamp(windowPos.x,
                canvasMinX - windowLeftOffset,
                canvasMaxX - windowRightOffset);
            var clampedY = Mathf.Clamp(windowPos.y,
                canvasMinY - windowBottomOffset,
                canvasMaxY - windowTopOffset);

            rectTransform.anchoredPosition = new Vector2(clampedX, clampedY);
        }
    }
}