using System;
using System.Collections.Generic;
using TMPro;
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
    public TextMeshProUGUI numItemsText;

    public void Start()
    {
        if (winRect == null || !winRect.gameObject.activeSelf)
            return;

        CalcTotalSlots();
        UpdateGrid();
    }

    public void OnValidate()
    {
        if (numItemsText == null)
            return;

        numItemsText.text = $"{numItems}/100";
    }

    private void UpdateGrid()
    {
        for (int i = 0; i < numSlots; i++)
        {
            if (items.Count < numSlots)
            {
                var item = Instantiate(slotPrefab, transform);
                items.Add(item);

                if (i < numItems)
                {
                    items[i].itemAmount = 1;
                    items[i].OnValidate();
                }
            }
        }
    }

    public void OnGridChange()
    {
        if (winRect == null || !winRect.gameObject.activeSelf)
            return;

        ClearSlots();
        CalcTotalSlots();
        UpdateGrid();
    }

    private void CalcTotalSlots()
    {
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
        for (int i = 0; i < numSlots; i++)
        {
            var index = numSlots - i - 1;
            var obj = transform.GetChild(index);
            Destroy(obj.gameObject);
        }

        items.Clear();
    }
}
