using RO_Flex_UI.Panels;
using RO_Flex_UI.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Components
{
    public class Resizable : MonoBehaviour, IComponent, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public class ResizeEvent : UnityEvent<PointerEventData> { }

        [SerializeField] private RectTransform targetTransform;
        [SerializeField] private Vector2 minSize = new(100, 100);
        [SerializeField] private Vector2 maxSize = new(400, 400);
        [SerializeField] private bool snapToStep = false;
        [SerializeField] private bool ignoreAnchor = false;
        [SerializeField] private Vector2 stepSize = new(50, 50);
        [SerializeField] private Vector2 borderOffset = new(0, 0); // FIXME calculate this

        private Vector2 startMousePos;
        private Vector2 startWinPos;
        private Vector2 startWinSize;

        public ResizeEvent onBeginResize;
        public ResizeEvent onResize;
        public ResizeEvent onEndResize;

        private void Start()
        {
            if (!EnsureReferences()) return;
        }

        public bool EnsureReferences()
        {
            if (targetTransform == null)
            {
                Tools.LogMissingReference(this, nameof(targetTransform));
                return false;
            }

            return true;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (targetTransform == null) return;

            startMousePos = eventData.position;
            startWinPos = targetTransform.anchoredPosition;
            startWinSize = targetTransform.sizeDelta;

            onBeginResize?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (targetTransform == null) return;

            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            eventData.useDragThreshold = false;

            // grabs mouse difference and inverts Y axis
            var mouseDelta = (eventData.position - startMousePos) * new Vector2(1, -1);
            var newWinSize = startWinSize + mouseDelta / targetTransform.transform.lossyScale;

            if (snapToStep)
            {
                var stepCount = Vector2Int.RoundToInt((newWinSize - borderOffset) / stepSize);
                newWinSize = stepCount * stepSize + borderOffset;
            }

            newWinSize.x = Mathf.Clamp(newWinSize.x, minSize.x, maxSize.x);
            newWinSize.y = Mathf.Clamp(newWinSize.y, minSize.y, maxSize.y);

            targetTransform.sizeDelta = newWinSize;

            // Keep the window fixed relative to its anchor point when resizing.
            // ignoreAnchor will always uses a top-left anchor point
            var dSize = newWinSize - startWinSize;
            var anchorPoint = ignoreAnchor ? new Vector2(0f, 1f) : (targetTransform.anchorMin + targetTransform.anchorMax) * 0.5f;
            var pivotOffset = targetTransform.pivot - anchorPoint;

            targetTransform.anchoredPosition = startWinPos + Vector2.Scale(dSize, pivotOffset);

            onResize?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onEndResize?.Invoke(eventData);
        }

        #region Getter & Setter
        public Vector2 MinSize
        {
            get => minSize;
            set => minSize = value;
        }
        public Vector2 MaxSize
        {
            get => maxSize;
            set => maxSize = value;
        }
        public Vector2 StepSize
        {
            get => stepSize;
            set => stepSize = value;
        }
        #endregion
    }
}