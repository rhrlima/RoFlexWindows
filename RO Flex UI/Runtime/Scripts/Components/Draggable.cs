using UnityEngine;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Components
{
    public class Draggable : DraggableBase
    {
        public override void HandleEndDrag(PointerEventData eventData)
        {
            if (returnToOrigin)
            {
                draggedRect.anchoredPosition = originPosition;
            }
            else
            {
                draggedRect.anchoredPosition = Vector2Int.RoundToInt(draggedRect.anchoredPosition);
            }
        }

        public override void HandleOnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!isBeingDragged)
                return;

            draggedRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public override void HandleStartDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;

            StoreOriginPosition();

            draggedRect.transform.SetAsLastSibling();
        }


    }
}