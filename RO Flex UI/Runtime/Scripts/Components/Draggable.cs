using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Components
{
    public class Draggable : MonoBehaviour, IComponent, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [Serializable]
        public class DragEvent : UnityEvent<PointerEventData> { }

        [Tooltip("Transform to be dragged. Defaults to current GameObject.")]
        [SerializeField] private RectTransform targetTransform;
        [SerializeField] private bool returnToOrigin;
        private Canvas canvas;
        private Vector2 originPosition;
        private bool dragging = false;

        public DragEvent onBeginDrag;
        public DragEvent onDrag;
        public DragEvent onEndDrag;

        public void Start()
        {
            if (!EnsureReferences()) return;
        }

        public bool EnsureReferences()
        {
            canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.LogWarning($"[{name}] Draggable must be placed inside a Canvas.");
                return false;
            }

            if (targetTransform == null)
            {
                targetTransform = transform as RectTransform;
            }

            return targetTransform != null;
        }

        private void StoreOriginPosition()
        {
            if (targetTransform != null)
                originPosition = targetTransform.anchoredPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!EnsureReferences()) return;

            if (canvas == null) return;
            if (targetTransform == null) return;

            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            eventData.useDragThreshold = false;

            StoreOriginPosition();

            //FIXME maybe remove this from here, and add to Window Manager
            targetTransform.transform.SetAsLastSibling();

            dragging = true;

            onBeginDrag?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (canvas == null || targetTransform == null) return;

            if (!dragging) return;

            targetTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

            onDrag?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!dragging) return;

            dragging = false;

            if (targetTransform == null) return;

            if (returnToOrigin)
                targetTransform.anchoredPosition = originPosition;

            onEndDrag?.Invoke(eventData);
        }

        #region Getter & Setter
        public bool ReturnToOrigin
        {
            get => returnToOrigin;
            set => returnToOrigin = value;
        }
        public bool Dragging => dragging;
        #endregion
    }
}