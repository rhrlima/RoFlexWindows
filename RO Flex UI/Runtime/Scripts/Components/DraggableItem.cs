using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace RO_Flex_UI.Components
{
    public readonly struct DragPayload
    {
        public DragPayload(
            IDraggable draggable,
            Vector2 originPosition,
            object data,
            object context,
            IDragVisual sourceVisual)
            : this(
                draggable,
                originPosition,
                data,
                context,
                sourceVisual,
                sourceVisual != null ? sourceVisual.CapturePresentation() : default)
        {
        }

        public DragPayload(
            IDraggable draggable,
            Vector2 originPosition,
            object data,
            object context,
            IDragVisual sourceVisual,
            DragPresentation presentation)
        {
            Draggable = draggable;
            OriginPosition = originPosition;
            Data = data;
            Context = context;
            SourceVisual = sourceVisual;
            Presentation = presentation;
        }

        public IDraggable Draggable { get; }
        public Vector2 OriginPosition { get; }
        public object Data { get; }
        public object Context { get; }
        public IDragVisual SourceVisual { get; }
        public DragPresentation Presentation { get; }
        public Sprite Sprite => Presentation.Sprite;
        public string Amount => Presentation.Amount ?? string.Empty;
        public string Text => Presentation.Text ?? string.Empty;

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

        public bool TryGetContext<T>(out T value)
        {
            if (Context is T typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryGetSourceVisual<T>(out T value) where T : class, IDragVisual
        {
            if (SourceVisual is T typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }
    }

    public class DraggableItem : MonoBehaviour, IComponent, IDraggable, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Serializable] public class DragEvent : UnityEvent<PointerEventData> { }
        [Serializable] public class DropResultEvent : UnityEvent<DraggableItem> { }

        [Header("Drag Proxy")]
        [FormerlySerializedAs("target")]
        [FormerlySerializedAs("proxyItem")]
        [SerializeField] private MonoBehaviour proxyVisualComponent;

        [Header("Drag Source")]
        [SerializeField] private MonoBehaviour sourceVisualComponent;

        [Header("Drag Events")]
        public DragEvent onBeginDrag = new();
        public DragEvent onDrag = new();
        public DragEvent onEndDrag = new();

        [Header("Drop Events")]
        public DropResultEvent onDropAccepted = new();
        public DropResultEvent onDropRejected = new();

        private IDragVisual sourceVisual;
        private IDragVisual proxyVisual;
        private Canvas canvas;
        private RectTransform sourceRect;
        private RectTransform targetRect;
        private RectTransform targetParent;
        private CanvasGroup targetCanvasGroup;
        private object data;
        private object context;
        private DragPresentation? presentation;
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

            proxyVisual.SetActive(false);
        }

        private void OnDisable()
        {
            if (!dragging)
                return;

            FinishDrag(DropResult.Rejected, false);
            dragging = false;
        }

        public void Configure(object data, object context = null, DragPresentation? presentation = null)
        {
            this.data = data;
            this.context = context;
            this.presentation = presentation;
        }

        public bool EnsureReferences()
        {
            sourceVisualComponent ??= FindDragVisualComponent(gameObject);
            sourceVisual = sourceVisualComponent as IDragVisual;
            proxyVisual = proxyVisualComponent as IDragVisual;
            canvas ??= GetComponentInParent<Canvas>();
            sourceRect = sourceVisual?.RectTransform;
            targetRect = proxyVisual?.RectTransform;
            targetParent = targetRect != null ? targetRect.parent as RectTransform : null;

            if (sourceVisual == null
                || proxyVisual == null
                || canvas == null
                || sourceRect == null
                || targetRect == null
                || targetParent == null)
            {
                return false;
            }

            if (!sourceVisual.EnsureReferences() || !proxyVisual.EnsureReferences())
                return false;

            targetCanvasGroup = proxyVisualComponent.GetComponent<CanvasGroup>();
            if (targetCanvasGroup == null)
                targetCanvasGroup = proxyVisualComponent.gameObject.AddComponent<CanvasGroup>();

            targetCanvasGroup.blocksRaycasts = false;
            targetCanvasGroup.interactable = false;
            return true;
        }

        private static MonoBehaviour FindDragVisualComponent(GameObject target)
        {
            var behaviours = target.GetComponents<MonoBehaviour>();
            foreach (var behaviour in behaviours)
            {
                if (behaviour is IDragVisual)
                    return behaviour;
            }

            return null;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData == null || eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!EnsureReferences() || !sourceVisual.IsVisible)
                return;

            var dragPresentation = presentation ?? sourceVisual.CapturePresentation();
            if (!proxyVisual.TryApplyPresentation(dragPresentation))
            {
                Debug.LogWarning(
                    $"[{name}] Drag proxy could not apply the payload presentation.",
                    this);
                return;
            }

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
                context,
                sourceVisual,
                dragPresentation);

            targetRect.anchoredPosition = originPosition;
            proxyVisual.SetActive(true);
            sourceVisual.SetActive(false);
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
                FinishDrag(DropResult.Rejected, true);

            dragging = false;
            onEndDrag?.Invoke(eventData);
        }

        public bool TryDrop(IDropZone dropZone)
        {
            if (!dragging || dropResolved || dropZone == null)
                return false;

            var result = DropResult.Rejected;
            try
            {
                if (dropZone.CanDrop(currentPayload))
                    result = dropZone.Drop(currentPayload);

                return result.Accepted;
            }
            finally
            {
                FinishDrag(result, true);
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

        private void FinishDrag(DropResult result, bool notify)
        {
            if (dropResolved)
                return;

            dropResolved = true;

            if (result.Accepted && result.SourceDisposition == DragSourceDisposition.Clear)
            {
                sourceVisual.Clear();
                sourceVisual.SetActive(false);
                data = null;
                presentation = null;
            }
            else
            {
                targetRect.anchoredPosition = currentPayload.OriginPosition;
                sourceVisual.SetActive(true);
            }

            proxyVisual.SetActive(false);

            if (!notify)
            {
                dragging = false;
                return;
            }

            if (result.Accepted)
                onDropAccepted?.Invoke(this);
            else
                onDropRejected?.Invoke(this);
        }
    }
}
