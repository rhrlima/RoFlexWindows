using RO_Flex_UI.Components.DragAndDrop;
using UnityEngine;

public class DropArea : MonoBehaviour, IDragTarget
{
    public bool ConsumeSource = false;
    public bool CanDrop(DragPayload payload)
    {
        return payload != null && (payload.data is ItemData || payload.data is SkillData);
    }

    public void OnDropComplete(DragPayload payload)
    {
        var text = string.Empty;
        switch (payload.data)
        {
            case ItemData itemData:
                text = $"{itemData.name}: {itemData.amount} un.";
                break;
            case SkillData skillData:
                text = $"{skillData.name}: {skillData.currLevel}/{skillData.maxLevel}";
                break;
            default:
                text = payload.text;
                break;
        }
        Debug.Log($"[{name}] Dropped payload: {text}");

        if (ConsumeSource)
        {
            if (payload.source is DraggableItem source)
            {
                source.Clear();
            }
        }
    }
}
