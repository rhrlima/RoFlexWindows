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
        [SerializeField] private Vector2 stepSize = new(50, 50);
        [SerializeField] private Vector2 offset = new(40, 50);

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

                window = parent.transform as RectTransform;
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
                var stepCount = Vector2Int.RoundToInt((newWinSize - offset) / stepSize);
                newWinSize = stepCount * stepSize + offset;
            }

            newWinSize.x = Mathf.Clamp(newWinSize.x, minSize.x, maxSize.x);
            newWinSize.y = Mathf.Clamp(newWinSize.y, minSize.y, maxSize.y);

            window.sizeDelta = newWinSize;

            // fix anchor point
            var dSize = newWinSize - startWinSize;
            var wp = window.pivot;

            window.anchoredPosition = startWinPos + new Vector2(wp.x * dSize.x, -wp.y * dSize.y);

            OnResize.Invoke();
        }
    }
}