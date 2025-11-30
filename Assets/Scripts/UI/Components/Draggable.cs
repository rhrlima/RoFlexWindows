using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Canvas canvas;
    private RectTransform window;
    private bool isBeingDragged = false;

    public void Start()
    {
        // Canvas ref for scaling calculations
        canvas = FindAnyObjectByType<Canvas>();
        // Window ref that will be dragged
        window = GetComponentInParent<Window>().transform as RectTransform;

        if (window == null)
            Debug.LogError("Draggable component must be a child of a Window component.");
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
        window.SetAsLastSibling();
        StartDrag();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (window == null) return;

        window.anchoredPosition = Vector2Int.RoundToInt(window.anchoredPosition);
        EndDrag();
    }
}
