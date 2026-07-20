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

            if (eventData == null)
                return true;

            // if (eventData.dragging)
            //     return false;

            var draggedItem = eventData.pointerDrag != null
                ? eventData.pointerDrag.GetComponentInParent<DraggableItem>()
                : null;

            return draggedItem == null;// || !draggedItem.Dragging;
        }
    }
}