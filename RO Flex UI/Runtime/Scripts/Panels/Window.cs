using RO_Flex_UI.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Panels
{
    public class Window : MonoBehaviour, IWindow, IPointerDownHandler
    {
        [Header("Window Settings")]
        [SerializeField] private bool resetToCenter;
        [SerializeField] private bool isDraggable;
        [SerializeField] private bool isResizable;

        private Draggable draggableComponent;
        private Resizable resizeComponent;

        RectTransform IWindow.transform => transform as RectTransform;

        protected virtual void Awake()
        {
            // if components are null, behavior is ignored
            draggableComponent = GetComponentInChildren<Draggable>(true);
            resizeComponent = GetComponentInChildren<Resizable>(true);

            OnValidate();
        }

        private void OnEnable()
        {
            if (resetToCenter)
                CenterWindow();
            else
                FitWindowIntoPlayArea();
        }

        private void OnValidate()
        {
            ToggleDraggable();
            ToggleResisable();
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
            // TODO manager to close using ESC
            // var mgr = WindowManager.GetInstance();
            // if (!mgr.Contains(this))
            //     mgr.PushWindow(this);

            // bring it forward
            transform.SetAsLastSibling();

            if (!isActiveAndEnabled)
                gameObject.SetActive(true);
        }

        public virtual void HideWindow()
        {
            gameObject.SetActive(false);
        }

        public void CenterWindow()
        {
            var canvas = gameObject.transform.parent as RectTransform;
            transform.position = canvas.rect.size / 2f;
        }

        public void FitWindowIntoPlayArea()
        {
            //TODO
            // Debug.LogWarning("Window.FitWindowIntoPlayArea NOT IMPLEMENTED");
        }
    }
}