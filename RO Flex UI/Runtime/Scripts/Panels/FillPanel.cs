using RO_Flex_UI.Panels;
using RO_Flex_UI.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class FillPanel2 : MonoBehaviour, IPanel
{
    [Header("References")]
    [SerializeField] private RectTransform viewportRect;
    [SerializeField] private RectTransform contentRect;
    [FormerlySerializedAs("slotTemplate")]
    [SerializeField] private GameObject cellTemplate;

    [Header("Configuration")]
    [SerializeField] private int maxSlots = 100;
    [FormerlySerializedAs("overflowCells")]
    [SerializeField] private int filledCells;

    private GridLayoutGroup gridLayout;
    private readonly List<GameObject> cells = new();
    private bool hasPendingGridChange;
    private bool isUpdatingGrid;
    private Vector2 lastViewportSize;

    public int Columns { get; private set; }
    public int Rows { get; private set; }
    public int TotalCells { get; private set; }
    public int FilledCells => filledCells;
    public int EmptyCells => Mathf.Max(0, TotalCells - filledCells);

    private void Start()
    {
        if (!EnsureReferences())
            return;

        StartCoroutine(InitializeGrid());
    }

    private IEnumerator InitializeGrid()
    {
        yield return null;
        Refresh();
    }

    private void Update()
    {
        if (viewportRect != null && Tools.GetRectSize(viewportRect) != lastViewportSize)
            hasPendingGridChange = true;

        if (!hasPendingGridChange)
            return;

        hasPendingGridChange = false;
        Refresh();
    }

    private void OnRectTransformDimensionsChange()
    {
        hasPendingGridChange = true;
    }

    public bool EnsureReferences()
    {
        if (contentRect == null)
            contentRect = transform as RectTransform;

        if (viewportRect == null)
            viewportRect = contentRect;

        if (gridLayout == null)
            gridLayout = GetComponent<GridLayoutGroup>();

        if (cellTemplate != null && cellTemplate.transform.parent == transform)
            cellTemplate.SetActive(false);

        if (contentRect == null)
        {
            Tools.LogMissingReference(this, nameof(contentRect));
            return false;
        }

        if (viewportRect == null)
        {
            Tools.LogMissingReference(this, nameof(viewportRect));
            return false;
        }

        if (gridLayout == null)
        {
            Tools.LogMissingReference(this, nameof(gridLayout));
            return false;
        }

        if (cellTemplate == null)
        {
            Tools.LogMissingReference(this, nameof(cellTemplate));
            return false;
        }

        return true;
    }

    public void SetFilledCells(int value)
    {
        filledCells = Mathf.Max(0, value);
        hasPendingGridChange = true;
    }

    public void Refresh()
    {
        if (!isActiveAndEnabled || isUpdatingGrid || !EnsureReferences())
            return;

        isUpdatingGrid = true;

        try
        {
            CalculateGrid();
            UpdateSlots();
        }
        finally
        {
            isUpdatingGrid = false;
        }
    }

    private void CalculateGrid()
    {
        lastViewportSize = Tools.GetRectSize(viewportRect);

        var padding = gridLayout.padding;
        var cellSize = gridLayout.cellSize;
        var spacing = gridLayout.spacing;

        var availableWidth = Mathf.Max(0f, lastViewportSize.x - padding.horizontal);
        var availableHeight = Mathf.Max(0f, lastViewportSize.y - padding.vertical);

        var fittedColumns = CalculateFitCount(availableWidth, cellSize.x, spacing.x);
        var fittedRows = CalculateFitCount(availableHeight, cellSize.y, spacing.y);
        var baseCapacity = fittedColumns * fittedRows;

        Columns = fittedColumns;
        Rows = fittedRows;
        TotalCells = baseCapacity;

        if (filledCells <= baseCapacity)
            return;

        var rowExpandedRows = Mathf.CeilToInt((float)filledCells / fittedColumns);
        var columnExpandedColumns = Mathf.CeilToInt((float)filledCells / fittedRows);

        var rowExpandedAspect = GetGridAspect(fittedColumns, rowExpandedRows, cellSize, spacing, padding);
        var columnExpandedAspect = GetGridAspect(columnExpandedColumns, fittedRows, cellSize, spacing, padding);
        var viewportAspect = lastViewportSize.y > 0f ? lastViewportSize.x / lastViewportSize.y : 1f;

        var rowAspectDelta = Mathf.Abs(rowExpandedAspect - viewportAspect);
        var columnAspectDelta = Mathf.Abs(columnExpandedAspect - viewportAspect);

        if (rowAspectDelta <= columnAspectDelta)
        {
            Rows = rowExpandedRows;
        }
        else
        {
            Columns = columnExpandedColumns;
        }

        TotalCells = Columns * Rows;
    }

    private static int CalculateFitCount(float availableSize, float cellSize, float spacing)
    {
        if (cellSize <= 0f)
            return 1;

        return Mathf.Max(1, Mathf.FloorToInt((availableSize + spacing) / (cellSize + spacing)));
    }

    private static float GetGridAspect(int columns, int rows, Vector2 cellSize, Vector2 spacing, RectOffset padding)
    {
        var size = new Vector2(
            columns * cellSize.x + Mathf.Max(0, columns - 1) * spacing.x + padding.horizontal,
            rows * cellSize.y + Mathf.Max(0, rows - 1) * spacing.y + padding.vertical);
        return size.y > 0f ? size.x / size.y : 1f;
    }

    private void UpdateSlots()
    {
        var targetSlots = Mathf.Min(Mathf.Max(1, maxSlots), Mathf.Max(1, TotalCells));

        while (cells.Count < targetSlots)
        {
            var cell = Instantiate(cellTemplate, contentRect);
            cell.SetActive(false);
            cells.Add(cell);
        }

        for (var i = 0; i < cells.Count; i++)
            cells[i].SetActive(i < targetSlots && i < TotalCells);
    }
}