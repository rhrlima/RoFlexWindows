using RO_Flex_UI.Components;
using System.Globalization;
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
            draggableItem.Configure(iconAmount, iconAmount, ParseAmount(iconAmount.Text));
    }

    public override bool CanDrop(DragPayload payload)
    {
        return acceptDrops
            && payload.Item != null
            && payload.Item.gameObject != gameObject
            && payload.Sprite != null
            && payload.Amount > 0
            && iconAmount != null
            && iconAmount.Sprite == null;
    }

    public override bool Drop(DragPayload payload)
    {
        if (!CanDrop(payload) || !payload.TryGetSource<IconAmount>(out var source))
            return false;

        iconAmount.Set(payload.Sprite, payload.Amount);
        source.Clear();

        var destinationDraggable = GetComponent<DraggableItem>();
        if (destinationDraggable != null)
            destinationDraggable.Configure(payload.Data, iconAmount, payload.Amount);

        payload.Item.Configure(null, source, 0);
        return true;
    }

    private static int ParseAmount(string value)
    {
        return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var amount)
            ? Mathf.Max(0, amount)
            : 0;
    }

    protected override void NotifyDropAccepted(DraggableItem item)
    {
        if (statusText != null)
            statusText.text = $"Accepted: {item.name}";

        Debug.Log($"[{name}] Accepted {item.name}.", this);
        base.NotifyDropAccepted(item);
    }

    protected override void NotifyDropRejected(DraggableItem item)
    {
        if (statusText != null)
            statusText.text = $"Rejected: {item.name}";

        Debug.Log($"[{name}] Rejected {item.name}.", this);
        base.NotifyDropRejected(item);
    }
}
