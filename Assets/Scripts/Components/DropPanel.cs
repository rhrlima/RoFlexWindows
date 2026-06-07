using RO_Flex_UI.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPanel : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggable != null)
        {
            draggable.EndDrag();
            draggable.transform.SetParent(transform);

            var dragTransform = draggable.transform as RectTransform;
            dragTransform.anchoredPosition = Vector2.zero;
        }
    }
}
