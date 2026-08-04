using UnityEngine;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Components.DragAndDrop
{
    public class DragTrigger : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        protected IDragSource source;

        protected virtual void Start()
        {
            source = GetComponentInParent<IDragSource>();

            if (source == null)
                Debug.LogError($"[{name}] Could not find an IDragSource presenter.");
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (source == null) return;

            DraggableManager.Instance.BeginDragSession(source, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (source == null) return;

            DraggableManager.Instance.OnEndDragSession(source, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (source == null) return;

            DraggableManager.Instance.OnDrag(source, eventData);
        }
    }
}