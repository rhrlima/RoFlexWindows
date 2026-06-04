using RO_Flex_UI.Panels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Components
{
    public class Resizable : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        [SerializeField] private Vector2 minSize = new(100, 100);
        [SerializeField] private Vector2 maxSize = new(400, 400);
        [SerializeField] private bool snapToStep = false;
        [SerializeField] private bool ignoreAnchor = false;
        [SerializeField] private Vector2 stepSize = new(50, 50);
        [SerializeField] private Vector2 borderOffset = new(0, 0);

        [Tooltip("Optional. If not set, will try to find a Window component in parents.")]
        [SerializeField] private RectTransform window;

        [Space]
        [SerializeField] private UnityEvent OnResize;
        private Vector2 startMousePos;
        private Vector2 startWinPos;
        private Vector2 startWinSize;

        private void Awake()
        {
            if (window == null)
            {
                var parent = GetComponentInParent<IWindow>(true);

                if (parent == null)
                {
                    Debug.LogError("Resizable must be a child of a Window.");
                    return;
                }

                window = parent.transform;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (window == null) return;

            startMousePos = eventData.position;
            startWinPos = window.anchoredPosition;
            startWinSize = window.sizeDelta;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (window == null) return;

            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            eventData.useDragThreshold = false;

            // grabs mouse difference and inverts Y axis
            var mouseDelta = (eventData.position - startMousePos) * new Vector2(1, -1);
            var newWinSize = startWinSize + mouseDelta / window.transform.lossyScale;

            if (snapToStep)
            {
                var stepCount = Vector2Int.RoundToInt((newWinSize - borderOffset) / stepSize);
                newWinSize = stepCount * stepSize + borderOffset;
            }

            newWinSize.x = Mathf.Clamp(newWinSize.x, minSize.x, maxSize.x);
            newWinSize.y = Mathf.Clamp(newWinSize.y, minSize.y, maxSize.y);

            window.sizeDelta = newWinSize;

            // Keep the window fixed relative to its anchor point when resizing.
            // ignoreAnchor will always uses a top-left anchor point
            var dSize = newWinSize - startWinSize;
            var anchorPoint = ignoreAnchor ? new Vector2(0f, 1f) : (window.anchorMin + window.anchorMax) * 0.5f;
            var pivotOffset = window.pivot - anchorPoint;

            window.anchoredPosition = startWinPos + Vector2.Scale(dSize, pivotOffset);

            OnResize.Invoke();
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