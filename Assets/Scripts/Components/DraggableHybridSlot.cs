using RO_Flex_UI.Components;
using RO_Flex_UI.Components.DragAndDrop;
using UnityEngine;

public class DraggableHybridSlot : IconAmount, IDragSource, IDragTarget
{
    [SerializeField] protected ScriptableObject Data;
    [SerializeField] protected bool canReplaceOnDrop = false;

    protected override void Start()
    {
        base.Start();

        if (IsSupported(Data))
        {
            Assign(Sprite, GetSecondaryValue(Data));
        }
    }

    public DragPayload CreatePayload()
    {
        return new DragPayload(this, Sprite, GetPayloadText(Data), Data);
    }

    public bool CanDrag()
    {
        return IsSupported(Data);
    }

    public bool CanDrop(DragPayload payload)
    {
        return payload != null
            && IsSupported(payload.data)
            && (canReplaceOnDrop || Data == null);
    }

    public void OnDropComplete(DragPayload payload)
    {
        if (payload.data is not ScriptableObject data || !IsSupported(data))
        {
            return;
        }

        Data = data;
        Assign(payload.sprite, GetSecondaryValue(data));
        SetActive(true);
    }

    public void OnDragComplete()
    {
        Data = null;
        Clear();
    }

    private static bool IsSupported(object data)
    {
        return data is ItemData or SkillData;
    }

    private static string GetSecondaryValue(object data)
    {
        return data switch
        {
            ItemData item => item.amount.ToString(),
            SkillData skill => skill.currLevel.ToString(),
            _ => string.Empty
        };
    }

    private static string GetPayloadText(object data)
    {
        return data switch
        {
            ItemData item => $"{item.name}: {item.amount} un.",
            SkillData skill => $"{skill.name}: {skill.currLevel}/{skill.maxLevel}",
            _ => string.Empty
        };
    }
}
