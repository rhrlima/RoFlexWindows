using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Components
{
    public readonly struct DragPayload
    {
        public DraggableItem Item { get; }
        public Vector2 OriginPosition { get; }
        public object Data { get; }
        public object Source { get; }
        public Sprite Sprite { get; }
        public string Amount { get; }

        public DragPayload(
            DraggableItem item,
            Vector2 originPosition,
            object data,
            object source,
            Sprite sprite,
            string amount = "0")
        {
            Item = item;
            OriginPosition = originPosition;
            Data = data;
            Source = source;
            Sprite = sprite;
            Amount = amount;
        }

        public bool TryGetData<T>(out T value)
        {
            if (Data is T typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryGetSource<T>(out T value)
        {
            if (Source is T typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }
    }

    public class DraggableItem : MonoBehaviour, IComponent, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Serializable]
        public class DragEvent : UnityEvent<PointerEventData> { }

        [Serializable]
        public class DropResultEvent : UnityEvent<DraggableItem> { }

        [Header("Drag Proxy")]
        [SerializeField] private IconAmount target;

        [Header("Drag Events")]
        public DragEvent onBeginDrag = new();
        public DragEvent onDrag = new();
        public DragEvent onEndDrag = new();

        [Header("Drop Events")]
        public DropResultEvent onDropAccepted = new();
        public DropResultEvent onDropRejected = new();

        private IconAmount source;
        private Canvas canvas;
        private RectTransform sourceRect;
        private RectTransform targetRect;
        private RectTransform targetParent;
        private CanvasGroup targetCanvasGroup;
        private object data;
        private object sourceContext;
        private bool dragging;
        private bool dropResolved;
        private DragPayload currentPayload;

        public bool Dragging => dragging;
        public bool CanResolveDrop => dragging && !dropResolved;
        public DragPayload CurrentPayload => currentPayload;

        private void Start()
        {
            if (!EnsureReferences())
            {
                enabled = false;
                return;
            }

            target.SetActive(false);
        }

        private void OnDisable()
        {
            if (dragging)
            {
                FinishDrag(false, false);
                dragging = false;
            }
        }

        public void Configure(object itemData, object itemSource = null)
        {
            data = itemData;
            sourceContext = itemSource;
        }

        public bool EnsureReferences()
        {
            source ??= GetComponent<IconAmount>();
            canvas ??= GetComponentInParent<Canvas>();
            sourceRect ??= source != null ? source.transform as RectTransform : null;
            targetRect = target != null ? target.transform as RectTransform : null;
            targetParent = targetRect != null ? targetRect.parent as RectTransform : null;

            if (source == null || target == null || canvas == null || sourceRect == null || targetParent == null)
                return false;

            if (!source.EnsureReferences() || !target.EnsureReferences())
                return false;

            targetCanvasGroup = target.GetComponent<CanvasGroup>();
            if (targetCanvasGroup == null)
                targetCanvasGroup = target.gameObject.AddComponent<CanvasGroup>();

            targetCanvasGroup.blocksRaycasts = false;
            targetCanvasGroup.interactable = false;
            return true;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData == null || eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!EnsureReferences())
                return;

            if (!source.IsVisible) return;

            eventData.useDragThreshold = false;
            dragging = true;
            dropResolved = false;

            var eventCamera = eventData.pressEventCamera;
            var sourceScreenPosition = RectTransformUtility.WorldToScreenPoint(eventCamera, sourceRect.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                targetParent,
                sourceScreenPosition,
                eventCamera,
                out var originPosition);

            currentPayload = new DragPayload(
                this,
                originPosition,
                data,
                sourceContext ?? source,
                source.Sprite,
                source.Amount);

            target.Assign(source.Sprite, source.Amount);
            targetRect.anchoredPosition = originPosition;
            target.SetActive(true);
            source.SetActive(false);
            MoveProxy(eventData);

            onBeginDrag?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!dragging || eventData == null)
                return;

            MoveProxy(eventData);
            onDrag?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!dragging)
                return;

            if (!dropResolved)
                FinishDrag(false, true);

            dragging = false;

            onEndDrag?.Invoke(eventData);
        }

        public bool TryDrop(IDropZone dropZone)
        {
            if (!dragging || dropResolved || dropZone == null)
                return false;

            var accepted = false;
            try
            {
                accepted = dropZone.CanDrop(currentPayload) && dropZone.Drop(currentPayload);
                return accepted;
            }
            finally
            {
                FinishDrag(accepted, true);
            }
        }

        private void MoveProxy(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    targetParent,
                    eventData.position,
                    eventData.pressEventCamera,
                    out var localPosition))
            {
                targetRect.anchoredPosition = localPosition;
            }
        }

        private void FinishDrag(bool accepted, bool notify)
        {
            if (dropResolved)
                return;

            dropResolved = true;

            if (!accepted && targetRect != null)
                targetRect.anchoredPosition = currentPayload.OriginPosition;

            if (target != null)
                target.SetActive(false);

            if (source != null)
                source.SetActive(true);

            if (!notify)
            {
                dragging = false;
                return;
            }

            if (accepted)
                onDropAccepted?.Invoke(this);
            else
                onDropRejected?.Invoke(this);
        }
    }
}