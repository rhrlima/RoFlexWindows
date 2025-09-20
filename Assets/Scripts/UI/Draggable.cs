using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler
{
    private Canvas canvas;
    private RectTransform window;

    public void Start()
    {
        canvas = FindAnyObjectByType<Canvas>();
        window = transform.parent as RectTransform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        window.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}
