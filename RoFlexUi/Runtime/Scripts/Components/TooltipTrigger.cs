using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RO_Flex_UI.Components
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler
    {
        [SerializeField] private bool tooltipEnabled = true;
        [SerializeField] private TooltipBox tooltipComponent;
        [SerializeField] private string tooltipText;
        public bool Enabled
        {
            get => tooltipEnabled;
            set => tooltipEnabled = value;
        }
        public string TooltipText => tooltipText;
        public Action OnTrigger;

        private void OnDisable()
        {
            tooltipComponent?.HideTooltip();
        }

        public void SetText(string text)
        {
            tooltipText = text;
            tooltipComponent?.SetText(text);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnTrigger?.Invoke();

            if (!CanShowTooltip(eventData))
            {
                tooltipComponent?.HideTooltip();
                return;
            }

            tooltipComponent.SetText(tooltipText);
            tooltipComponent.ShowTooltip();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltipComponent?.HideTooltip();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            tooltipComponent?.HideTooltip();
        }

        private bool CanShowTooltip(PointerEventData eventData)
        {
            if (!tooltipEnabled || tooltipComponent == null)
                return false;

            // if (eventData != null)
            //     return false;

<<<<<<< HEAD:RoFlexUi/Runtime/Scripts/Components/TooltipTrigger.cs
            return true;
=======
            if (eventData.dragging)
                return false;

            var draggedItem = eventData.pointerDrag != null
                ? FindDraggable(eventData.pointerDrag)
                : null;

            return draggedItem == null || !draggedItem.Dragging;
>>>>>>> 4b59f84 (refac: Rework draggable):RO Flex UI/Runtime/Scripts/Components/TooltipTrigger.cs
        }

        private static IDraggable FindDraggable(GameObject pointerDrag)
        {
            var behaviours = pointerDrag.GetComponentsInParent<MonoBehaviour>(true);
            foreach (var behaviour in behaviours)
            {
                if (behaviour is IDraggable draggable)
                    return draggable;
            }

            return null;
        }
    }
}
