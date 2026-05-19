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

    private void Start()
    {
        var items = new List<string>();
        for (var i = 0; i < 10; i++)
        {
            items.Add($"Option {i + 1}");
        }

        listScrollPanel.SetOptions(items, (item, data) =>
            {
                item.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data;
            }
        );

        var items2 = new List<string>();
        for (var i = 0; i < 5; i++)
        {
            items2.Add($"Option {i + 1}");
        }
        listPanel.SetOptions(items2, (item, data) =>
            {
                item.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data;
            }
        );

        listMixedPrefabs.AddCustomObjects(mixPrefabs);
    }
}
