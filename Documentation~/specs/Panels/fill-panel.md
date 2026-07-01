# FillPanel — Integration Spec

[Back to specs index](../README.md)

## Purpose

Responsive grid (`FillPanel2` in `FillPanel.cs`) that calculates columns and rows from viewport size and instantiates a pool of cell GameObjects. Used for inventory grids and similar slot layouts.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `Columns` / `Rows` / `TotalCells` | `int` | Calculated grid dimensions (read-only). |
| `FilledCells` / `EmptyCells` | `int` | Filled count vs. capacity. |
| `SetFilledCells(int)` | Method | Sets how many cells count as filled (affects overflow expansion). |
| `Refresh()` | Method | Recalculates grid and activates/deactivates cells. |
| `EnsureReferences()` | `bool` | Validates viewport, content, grid layout, and cell template. |

Cells are private `GameObject` instances cloned from `cellTemplate`. No binding API.

Class is `FillPanel2` in the global namespace; tests resolve `"FillPanel, Assembly-CSharp"`.

## Desired Integration Pattern

```csharp
// Desired — bind each visible cell after refresh
fillPanel.SetFilledCells(inventory.Count);
fillPanel.Refresh();
fillPanel.ForEachCell((index, cell) =>
{
    if (index < inventory.Count)
        cell.GetComponent<IconAmount>().Assign(inventory[index].Icon, inventory[index].Amount);
    else
        cell.GetComponent<IconAmount>().Clear();
});
```

Prefer typed cell access (`GetCell(int)`, `IReadOnlyList<IconAmount>`) over raw `GameObject` and `GetComponent`.

## Related Scenarios

- [Inventory grid with drag-and-drop](../../integration-review.md#scenario-inventory-grid)

[Back to specs index](../README.md)
