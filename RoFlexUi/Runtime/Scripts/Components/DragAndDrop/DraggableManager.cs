using RO_Flex_UI.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RO_Flex_UI.Components.DragAndDrop
{
    public class DraggableManager : Singleton<DraggableManager>
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private IconAmount proxy;
        private RectTransform proxyTransform;
        private IDragSource activeSource;
        private DragPayload payload;
        private bool dragging;
        public bool Dragging => dragging;

        public void Start()
        {
            if (canvas == null)
            {
                canvas = GetComponentInParent<Canvas>();
                if (canvas == null)
                    Debug.LogError($"[{name}] No Canvas found.");
            }

            proxyTransform = proxy.transform as RectTransform;

            foreach (var graphic in proxy.GetComponentsInChildren<Graphic>(true))
                graphic.raycastTarget = false;
        }

        public bool TryGetPayload(out DragPayload payload)
        {
            if (dragging && this.payload != null)
            {
                payload = this.payload;
                return true;
            }

            payload = default;
            return false;
        }

        public bool BeginDragSession(IDragSource source, PointerEventData eventData)
        {
            if (dragging) return false;
            if (payload != null) return false;
            if (source == null) return false;
            if (eventData.button != PointerEventData.InputButton.Left) return false;
            if (!source.CanDrag()) return false;

            payload = source.CreatePayload();

            if (payload == null)
            {
                Debug.LogError("Payload cannot be null.");
                return false;
            }

            activeSource = source;
            dragging = true;

            proxy.Assign(payload.sprite, payload.text);
            proxy.SetActive(true);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                canvas.worldCamera,
                out var localPoint
            );

            proxyTransform.anchoredPosition = localPoint;

            return true;
        }

        public void OnEndDragSession(IDragSource source, PointerEventData eventData)
        {
            if (!dragging) return;
            if (!ReferenceEquals(activeSource, source)) return;

            dragging = false;

            proxy.Clear();
            proxy.SetActive(false);

            payload = null;
            activeSource = null;
        }

        public void OnDrag(IDragSource source, PointerEventData eventData)
        {
            if (!dragging) return;
            if (!ReferenceEquals(activeSource, source)) return;

            MoveProxy(eventData);
        }

        public bool TryDrop(IDragSource targetSource, IDragTarget target)
        {
            if (target == null) return false;
            if (!TryGetPayload(out var currentPayload)) return false;
            if (targetSource != null && ReferenceEquals(currentPayload.source, targetSource)) return false;
            if (!target.CanDrop(currentPayload)) return false;

            target.OnDropComplete(currentPayload);
            currentPayload.source.OnDragComplete();
            return true;
        }

        private void MoveProxy(PointerEventData eventData)
        {
            proxyTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }
}