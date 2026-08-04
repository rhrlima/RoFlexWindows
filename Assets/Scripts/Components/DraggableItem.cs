using RO_Flex_UI.Components;
using RO_Flex_UI.Components.DragAndDrop;
using UnityEngine;

public class DraggableItem : IconAmount, IDragSource, IDragTarget
{
    [SerializeField] protected ItemData Data;
    [SerializeField] protected bool canReplaceOnDrop = false;
    private TooltipTrigger tooltipTrigger;

    protected override void Start()
    {
        base.Start();
        if (Data != null)
            Assign(Sprite, Data.amount.ToString());

        tooltipTrigger = GetComponent<TooltipTrigger>();
        if (tooltipTrigger != null && Data != null)
            tooltipTrigger.SetText($"{Data.name}: {Data.amount} un.");
    }

    public DragPayload CreatePayload()
    {
        var text = $"{Data.name}: {Data.amount} un.";
        return new DragPayload(this, Sprite, text, Data);
    }

    public bool CanDrag()
    {
        return Data != null;
    }

    public bool CanDrop(DragPayload payload)
    {
        return payload != null
            && payload.TryGetData<ItemData>(out _)
            && (canReplaceOnDrop || Data == null);
    }

    public void OnDropComplete(DragPayload payload)
    {
        if (!payload.TryGetData<ItemData>(out var data)) return;

        Data = data;
        Assign(payload.sprite, data.amount.ToString());
        SetActive(true);

        if (tooltipTrigger != null && Data != null)
        {
            tooltipTrigger.SetText($"{Data.name}: {Data.amount} un.");
            tooltipTrigger.Enabled = true;
        }
    }

    public void OnDragComplete()
    {
    }

    public override void Clear()
    {
        base.Clear();
        Data = null;
        if (tooltipTrigger != null)
            tooltipTrigger.Enabled = false;
    }
}