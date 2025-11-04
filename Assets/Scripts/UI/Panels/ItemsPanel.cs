using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemsPanel : MonoBehaviour
{
    [SerializeField] private RectTransform winRect;
    public int numSlots;
    [SerializeField] private int _numItems;
    public int numItems
    {
        get { return _numItems; }
        set
        {
            if (_numItems != value)
            {
                _numItems = value;
                OnGridChange();
            }
        }
    }
    public ItemEntry slotPrefab;
    public List<ItemEntry> items;

    public void Start()
    {
        if (winRect == null || !winRect.gameObject.activeSelf)
            return;

        CalcTotalSlots();
        UpdateGrid();
    }

    private void UpdateGrid()
    {
        while (transform.childCount < numSlots)
        {
            var item = Instantiate(slotPrefab, transform);

            if (transform.childCount < items.Count)
                items.Add(item);

            // Debug.Log($"ADDING {transform.childCount}/{numSlots}");
        }

        for (int i = 0; i < numItems; i++)
        {
            items[i].itemAmount = 1;
            items[i].Refresh();
        }
    }

    public void Update()
    {
        // OnGridChange();
    }

    public void OnGridChange()
    {
        // try to avoid redo all for disabled panels
        if (!isActiveAndEnabled)
            return;

        if (winRect == null || !winRect.gameObject.activeSelf)
            return;

        ClearSlots();
        CalcTotalSlots();
        UpdateGrid();
    }

    private void CalcTotalSlots()
    {
        //FIXME improve offset calculation 40,50

        int cols = Mathf.FloorToInt((winRect.sizeDelta.x - 40) / 32);

        int panelHeight = (int)Mathf.Max(
            winRect.sizeDelta.y,
            Mathf.CeilToInt((float)numItems / cols) * 32 + 50
        );

        int rows = Math.Max(
            Mathf.FloorToInt((panelHeight - 50) / 32),
            Mathf.CeilToInt((float)numItems / cols)
        );

        numSlots = Mathf.Max(Mathf.CeilToInt((float) numItems / cols) * cols, rows * cols);
    }

    private void ClearSlots()
    {
        // delete extra slots

        while (transform.childCount > numSlots)
        {
            var index = transform.childCount - 1;
            var obj = transform.GetChild(index);
            DestroyImmediate(obj.gameObject);

            // Debug.Log($"DELETING {transform.childCount}/{numSlots}");
        }

        // for (int i = 0; i < numSlots; i++)
        // {
        //     var index = numSlots - i - 1;
        //     var obj = transform.GetChild(index);
        //     DestroyImmediate(obj.gameObject);
        //     Debug.Log($"DELETING {transform.childCount}/{numSlots}");
        // }

        // items.Clear();
    }
}
