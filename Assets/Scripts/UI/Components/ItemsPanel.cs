using System.Collections.Generic;
using UnityEngine;

public class ItemsPanel : MonoBehaviour
{
    public int numItems;
    public GameObject entryPrefab;
    public List<GameObject> slots;
    public void Start()
    {
        UpdateGrid();
    }

    private void UpdateGrid()
    {
        for (int i = 0; i < numItems; i++)
        {
            var tmpEntry = Instantiate(entryPrefab);
            tmpEntry.transform.SetParent(this.transform, false);
            slots.Add(tmpEntry);
        }
    }
}
