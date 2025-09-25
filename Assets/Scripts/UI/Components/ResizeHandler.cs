using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeHandle : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private RectTransform window;
    [SerializeField] private Vector2 minSize = new(100, 100);
    [SerializeField] private Vector2 maxSize = new(1000, 1000);
    [SerializeField] private bool snapToStep = false;
    [SerializeField] private Vector2 stepSize = new(50, 50);
    private Vector2 startMousePos;
    private Vector2 startWinPos;
    private Vector2 startWinSize;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startMousePos = eventData.position;
        startWinPos = window.anchoredPosition;
        startWinSize = window.sizeDelta;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        var mouseDelta = eventData.position - startMousePos;
        var newWinSize = startWinSize + new Vector2(
            mouseDelta.x / window.transform.lossyScale.x,
            -mouseDelta.y / window.transform.lossyScale.y);

        if (snapToStep)
        {
            var stepCount = new Vector2(
                newWinSize.x / stepSize.x,
                newWinSize.y / stepSize.y
            );
            var currStepSize = new Vector2Int(
                Mathf.RoundToInt(stepCount.x),
                Mathf.RoundToInt(stepCount.y)
            );
            newWinSize = currStepSize * stepSize;
        }

        newWinSize.x = Mathf.Clamp(newWinSize.x, minSize.x, maxSize.x);
        newWinSize.y = Mathf.Clamp(newWinSize.y, minSize.y, maxSize.y);

        window.sizeDelta = newWinSize;

        // fix anchor point
        var dSize = newWinSize - startWinSize;
        var wp = window.pivot;

        window.anchoredPosition = startWinPos + new Vector2(wp.x * dSize.x, -wp.y * dSize.y);
    }
}
