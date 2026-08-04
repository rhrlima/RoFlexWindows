using RO_Flex_UI.Components;
using RO_Flex_UI.Components.DragAndDrop;
using UnityEngine;

public class DraggableSkill : IconAmount, IDragSource, IDragTarget
{
    [SerializeField] protected SkillData Data;
    [SerializeField] protected bool canReplaceOnDrop = false;

    public DragPayload CreatePayload()
    {
        var text = $"{Data.name}: {Data.currLevel}/{Data.maxLevel}";
        return new DragPayload(this, Sprite, text, Data);
    }

    public bool CanDrag()
    {
        return Data != null;
    }

    public bool CanDrop(DragPayload payload)
    {
        return payload != null
            && payload.TryGetData<SkillData>(out _)
            && (canReplaceOnDrop || Data == null);
    }

    public void OnDropComplete(DragPayload payload)
    {
        if (!payload.TryGetData<SkillData>(out var data)) return;

        Data = data;
        Assign(payload.sprite, string.Empty);
        SetActive(true);
    }

    public void OnDragComplete()
    {
        Data = null;
        Clear();
    }
}
