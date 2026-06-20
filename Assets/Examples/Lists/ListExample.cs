using RO_Flex_UI.Components;
using RO_Flex_UI.Panels;
using System.Collections.Generic;
using UnityEngine;

public class ListExample : MonoBehaviour
{
    [Header("Simple scripted list with scroll")]
    public ListPanel listScrollPanel;
    [Header("Simple scripted list")]
    public ListPanel listPanel;
    [Header("List of mixed prefabs")]
    public ListPanel listMixedPrefabs;
    public List<GameObject> mixPrefabs;
    public ListPanel listGears;

    private void Start()
    {
        var items = new List<string>();
        for (var i = 0; i < 10; i++)
        {
            items.Add($"Option {i + 1}");
        }

        listScrollPanel.AddItems(items, (item, data) =>
            {
                item.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data;
            }
        );

        var items2 = new List<string>();
        for (var i = 0; i < 5; i++)
        {
            items2.Add($"Option {i + 1}");
        }
        listPanel.AddItems(items2, (item, data) =>
            {
                item.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data;
            }
        );

        var mixedItems = new List<ListItem>();
        foreach (var prefab in mixPrefabs)
        {
            if (prefab != null && prefab.TryGetComponent<ListItem>(out var item))
                mixedItems.Add(item);
        }
        listMixedPrefabs.AddItems(mixedItems);

        listGears.AddItems(items, (item, data) =>
        {
            var gearSlot = item.GetComponent<IconText>();
            gearSlot.Text = data;
        });
    }
}
