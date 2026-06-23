using RO_Flex_UI.Components;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler
{
    [SerializeField] private bool tooltipEnabled = true;
    [SerializeField] private Tooltip tooltipComponent;

    private IconAmount iconAmount;

    private void Awake()
    {
        iconAmount = GetComponent<IconAmount>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!CanShowTooltip(eventData))
        {
            tooltipComponent?.HideTooltip();
            return;
        }

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

    private void OnDisable()
    {
        tooltipComponent?.HideTooltip();
    }

    private bool CanShowTooltip(PointerEventData eventData)
    {
        if (!tooltipEnabled || tooltipComponent == null || (iconAmount != null && !iconAmount.IsVisible))
            return false;

        if (eventData == null)
            return true;

        if (eventData.dragging)
            return false;

        var draggedItem = eventData.pointerDrag != null
            ? eventData.pointerDrag.GetComponentInParent<DraggableItem>()
            : null;

        return draggedItem == null || !draggedItem.Dragging;
    }
}