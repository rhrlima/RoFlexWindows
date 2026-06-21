using UnityEngine;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Components
{
    public abstract class DraggableBase : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Tooltip("Object to be dragged. Defaults to the current GameObject.")]
        [SerializeField] protected RectTransform draggedRect;
        [SerializeField] protected bool returnToOrigin;

        protected Canvas canvas;
        protected Vector2 originPosition;
        protected bool dragging = false;

        public virtual void Start()
        {
            if (!EnsureReferences()) return;
        }

        private bool EnsureReferences()
        {
            canvas = FindAnyObjectByType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError($"[{name}] No Canvas found in the scene.");
                return false;
            }

            if (draggedRect == null)
            {
                draggedRect = transform as RectTransform;
            }

            return true;
        }

        protected void StoreOriginPosition()
        {
            if (draggedRect != null)
                originPosition = draggedRect.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (draggedRect == null) return;

            HandleOnDrag(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (draggedRect == null) return;

            dragging = true;
            HandleStartDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (draggedRect == null) return;

            dragging = false;
            HandleEndDrag(eventData);
        }

        public abstract void HandleStartDrag(PointerEventData eventData);
        public abstract void HandleEndDrag(PointerEventData eventData);
        public abstract void HandleOnDrag(PointerEventData eventData);

        public bool ReturnToOrigin
        {
            get => returnToOrigin;
            set => returnToOrigin = value;

        }
    }
}