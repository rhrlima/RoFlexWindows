# FillPanel

[Go Back](../README.md)

## Description

`FillPanel` maintains a grid of cloned cell templates sized to its viewport. It
fits a base row-and-column capacity, expands the grid when `filledCells` exceeds
that capacity, and caps active cells at `maxSlots`.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| `Cells` | `IReadOnlyList<GameObject>` | Cells instantiated by the panel. |
| `Columns` | `int` | Current calculated column count. |
| `Rows` | `int` | Current calculated row count. |
| `TotalCells` | `int` | Current grid capacity before the maximum-slot cap. |
| `FilledCells` | `int` | Requested occupied-cell count. |
| `EmptyCells` | `int` | Remaining capacity, never below zero. |

## Public API

### Methods

| Method | Description |
| --- | --- |
| `SetFilledCells(value)` | Stores a non-negative filled count and schedules a refresh. |
| `Refresh()` | Recalculates the grid and updates instantiated cells immediately. |
| `GetCell(index)` | Returns a cell or throws for an invalid instantiated-cell index. |
| `GetCell<T>(index)` | Returns component `T` from a cell, or `null` if absent. |
| `TryGetCell<T>(index, out cell)` | Safely finds component `T` at an index. |
| `GetCells<T>()` | Returns matching components from all instantiated cells. |

The panel requires a `GridLayoutGroup`. When omitted, content and viewport
references fall back to the panel's own `RectTransform`. The cell template is
hidden when it is a direct child of the panel.

## Examples

```csharp
fillPanel.SetFilledCells(inventory.Items.Count);
fillPanel.Refresh();

foreach (var slot in fillPanel.GetCells<IconAmount>())
    slot.Clear();
```

## Related Components

- [IconAmount](../Components/icon-amount.md)
- [DraggableItem](../Components/draggable-item.md)
- [DropZone](../Components/drop-zone.md)

[Go Back](../README.md)
