using RO_Flex_UI.Components;
using RO_Flex_UI.Components.DragAndDrop;
using UnityEngine;

public class DraggableSkillEntry : SkillEntry, IDragSource
{
    public SkillData Data;

    public void Start()
    {
        if (Data == null) return;

        Assign(
            Sprite,
            Data.name,
            Data.currLevel,
            Data.maxLevel,
            Data.cost,
            Data.passive,
            Data.fixedLevel
        );

        spriteButton.onClick.AddListener(() =>
        {
            Debug.Log($"[DRAGGABLE] Skill Clicked: {Data.name}");
        });
    }

    public DragPayload CreatePayload()
    {
        var text = $"{Data.name}: {Data.currLevel}/{Data.maxLevel}";
        return new DragPayload(this, Sprite, text, Data);
    }

    public bool CanDrag()
    {
        return Data != null;
    }

    public void OnDragComplete()
    {
    }
}
