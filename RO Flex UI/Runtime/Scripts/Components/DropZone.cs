using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Components
{
    public class DropZone : MonoBehaviour, IDropZone, IDropHandler
    {
        [Serializable]
        public class DropEvent : UnityEvent<MonoBehaviour> { }

        [Header("Drop Events")]
        public DropEvent onDropAccepted = new();
        public DropEvent onDropRejected = new();

        public virtual bool CanDrop(DragPayload payload)
        {
            return true;
        }

        public virtual DropResult Drop(DragPayload payload)
        {
            return DropResult.Move;
        }

        public void OnDrop(PointerEventData eventData)
        {
            var pointerDrag = eventData?.pointerDrag;
            if (pointerDrag == null)
                return;

            var draggableComponent = FindDraggable(pointerDrag, out var draggable);
            if (draggableComponent == null || !draggable.CanResolveDrop)
                return;

            if (draggable.TryDrop(this))
                NotifyDropAccepted(draggableComponent);
            else
                NotifyDropRejected(draggableComponent);
        }

        private static MonoBehaviour FindDraggable(GameObject pointerDrag, out IDraggable draggable)
        {
            var behaviours = pointerDrag.GetComponentsInParent<MonoBehaviour>(true);
            foreach (var behaviour in behaviours)
            {
                if (!(behaviour is IDraggable typedDraggable))
                    continue;

                draggable = typedDraggable;
                return behaviour;
            }

            draggable = null;
            return null;
        }

        protected virtual void NotifyDropAccepted(MonoBehaviour draggable)
        {
            onDropAccepted?.Invoke(draggable);
        }

        protected virtual void NotifyDropRejected(MonoBehaviour draggable)
        {
            onDropRejected?.Invoke(draggable);
        }
    }

    public abstract class DropZone<TData> : DropZone
    {
        public sealed override bool CanDrop(DragPayload payload)
        {
            return payload.TryGetData<TData>(out var data) && CanDrop(data, payload);
        }

        public sealed override DropResult Drop(DragPayload payload)
        {
            return payload.TryGetData<TData>(out var data)
                ? Drop(data, payload)
                : DropResult.Rejected;
        }

        protected virtual bool CanDrop(TData data, DragPayload payload)
        {
            return true;
        }

        protected abstract DropResult Drop(TData data, DragPayload payload);
    }
}
