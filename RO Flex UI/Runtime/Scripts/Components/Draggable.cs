using UnityEngine;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Components
{

    public class Draggable : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private Canvas canvas;
        [Tooltip("Optional. If not set, will try to find a Window component in parents.")]
        [SerializeField] private RectTransform window;
        private bool isBeingDragged = false;

        public void Start()
        {
            // Canvas ref for scaling calculations
            canvas = FindAnyObjectByType<Canvas>();

            if (window == null)
            {
                Debug.LogError("Draggable must have a RectTranform assigned.");
                // var parent = GetComponentInParent<IWindow>(true);
                // if (parent == null)
                // {
                //     Debug.LogError("Draggable must be a child of a IWindow.");
                //     return;
                // }

                // window = parent.transform as RectTransform;
            }
        }

        public void StartDrag()
        {
            isBeingDragged = true;
        }

        public void EndDrag()
        {
            isBeingDragged = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (window == null) return;

            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!isBeingDragged)
                return;

            window.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (window == null) return;

            eventData.useDragThreshold = false;

            window.transform.SetAsLastSibling();

            StartDrag();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (window == null) return;

            window.anchoredPosition = Vector2Int.RoundToInt(window.anchoredPosition);

            EndDrag();
        }
    }
}
