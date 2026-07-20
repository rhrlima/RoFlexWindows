using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Components
{
    public class DropZone : MonoBehaviour, IDropZone, IDropHandler
    {
        [Serializable]
        public class DropEvent : UnityEvent<DraggableItem> { }

        [Header("Drop Events")]
        public DropEvent onDropAccepted = new();
        public DropEvent onDropRejected = new();

        public virtual bool CanDrop(DragPayload payload)
        {
            return true;
        }

        public virtual bool Drop(DragPayload payload)
        {
            return true;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var pointerDrag = eventData?.pointerDrag;
            if (pointerDrag == null) return;

            var draggableItem = pointerDrag.GetComponentInParent<DraggableItem>();
            if (draggableItem == null) return;

            if (!draggableItem.CanResolveDrop) return;

            if (draggableItem.TryDrop(this))
                NotifyDropAccepted(draggableItem);
            else
                NotifyDropRejected(draggableItem);
        }

        protected virtual void NotifyDropAccepted(DraggableItem item)
        {
            onDropAccepted?.Invoke(item);
        }

        protected virtual void NotifyDropRejected(DraggableItem item)
        {
            onDropRejected?.Invoke(item);
        }
    }

}