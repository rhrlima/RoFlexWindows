using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class WinBase : MonoBehaviour, IPointerDownHandler
{
    private RectTransform window;

    void Awake()
    {
        window = transform.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left
         && eventData.button != PointerEventData.InputButton.Right)
            return;

        window.SetAsLastSibling();
    }
}
