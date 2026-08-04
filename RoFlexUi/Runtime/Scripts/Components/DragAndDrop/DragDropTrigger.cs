using UnityEngine;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Components.DragAndDrop
{
    public class DragDropTrigger : DragTrigger, IDropHandler
    {
        private IDragTarget target;

        protected override void Start()
        {
            base.Start();
            target = GetComponentInParent<IDragTarget>();

            if (target == null)
                Debug.LogError($"[{name}] Could not find an IDragTarget presenter.");
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (target == null) return;

            DraggableManager.Instance.TryDrop(source, target);
        }
    }
}