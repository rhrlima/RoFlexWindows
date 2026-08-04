using RO_Flex_UI.Components;
using TMPro;
using UnityEngine;

public class DropPanel : DropZone
{
    [SerializeField] private bool acceptDrops = true;
    [SerializeField] private bool clearOnStart;
    [SerializeField] private TMP_Text statusText;

    private IconAmount iconAmount;

    private void Awake()
    {
        iconAmount = GetComponent<IconAmount>();
        if (iconAmount == null)
            return;

        if (clearOnStart)
            iconAmount.Clear();

        var draggableItem = GetComponent<DraggableItem>();
        if (draggableItem != null)
            draggableItem.Configure(iconAmount, iconAmount);
    }

    public override bool CanDrop(DragPayload payload)
    {
        return acceptDrops
            && payload.Draggable is MonoBehaviour draggable
            && draggable.gameObject != gameObject
            && payload.Sprite != null
            && iconAmount != null
            && iconAmount.Sprite == null;
    }

    public override DropResult Drop(DragPayload payload)
    {
        if (!CanDrop(payload) || !iconAmount.TryApplyPresentation(payload.Presentation))
            return DropResult.Rejected;

        iconAmount.SetActive(true);

        var destinationDraggable = GetComponent<DraggableItem>();
        if (destinationDraggable != null)
            destinationDraggable.Configure(payload.Data, iconAmount, payload.Presentation);

        payload.Draggable.Configure(null, payload.SourceVisual);
        return DropResult.Move;
    }

    protected override void NotifyDropAccepted(MonoBehaviour item)
    {
        if (statusText != null)
            statusText.text = $"Accepted: {item.name}";

        Debug.Log($"[{name}] Accepted {item.name}.", this);
        base.NotifyDropAccepted(item);
    }

    protected override void NotifyDropRejected(MonoBehaviour item)
    {
        if (statusText != null)
            statusText.text = $"Rejected: {item.name}";

        Debug.Log($"[{name}] Rejected {item.name}.", this);
        base.NotifyDropRejected(item);
    }
}
