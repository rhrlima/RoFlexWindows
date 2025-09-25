using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Canvas canvas;
    private RectTransform window;
    public bool isBeingDragged = false;

    public void Start()
    {
        canvas = FindAnyObjectByType<Canvas>();
        window = transform.parent as RectTransform;
    }

    public void EndDrag()
    {
        isBeingDragged = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (!isBeingDragged)
            return;

        window.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        window.SetAsLastSibling();

        eventData.useDragThreshold = false;

        isBeingDragged = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        window.anchoredPosition = new(
            Mathf.Round(window.anchoredPosition.x),
            Mathf.Round(window.anchoredPosition.y)
        );
        EndDrag();
    }
}
