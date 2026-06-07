using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Tooltip tooltipComponent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipComponent.ShowTooltip();
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipComponent.HideTooltip();
    }
}
