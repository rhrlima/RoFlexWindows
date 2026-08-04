using UnityEngine;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Components.DragAndDrop
{
    public class DropTrigger : MonoBehaviour, IDropHandler
    {
        private IDragTarget target;

        private void Start()
        {
            target = GetComponentInParent<IDragTarget>();

            if (target == null)
                Debug.LogError($"[{name}] Could not find an IDragTarget presenter.");
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (target == null) return;

            var targetSource = GetComponentInParent<IDragSource>();
            DraggableManager.Instance.TryDrop(targetSource, target);
        }
    }
}