using UnityEngine;

public class InventoryTabGroup : TabGroup
{
    public override void OnTabEnter(TabButton button)
    {
        base.OnTabEnter(button);
        // panels[button.transform.GetSiblingIndex()].OnGrid

        Debug.Log("TEST");
    }
}