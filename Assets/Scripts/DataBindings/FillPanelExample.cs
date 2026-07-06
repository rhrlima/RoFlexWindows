using RO_Flex_UI.Components;
using RO_Flex_UI.Panels;
using UnityEngine;

public class FillPanelExample : MonoBehaviour
{
    public FillPanel panel;

    public void Start()
    {
        var items = ItemsDatabase.items;
        panel.SetFilledCells(items.Count);
        panel.Refresh();

        for (var i = 0; i < items.Count; i++)
        {
            if (!panel.TryGetCell<IconAmount>(i, out var cell))
                continue;

            var item = items[i];
            cell.Assign(item.sprite, item.amount.ToString());
        }
    }
}
