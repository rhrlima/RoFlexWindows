# ListItem — Integration Spec

[Back to specs index](../README.md)

## Purpose

A selectable, focusable row inside a `ListPanel`. Handles pointer focus, keyboard/gamepad submit, and double-click activation. Subclass or configure child content to show row data.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `OnItemFocused` | `UnityEvent` | Invoked when the row receives focus. |
| `OnItemActivated` | `UnityEvent` | Invoked on submit or double-click. |
| `TargetButton` | `RoButton` | Underlying selectable button. |
| `EnsureReferences()` | `bool` | Resolves the required `RoButton`. |
| `BindToPanel(ListPanel)` | Method | Registers this row with its host panel. |
| `FocusItem()` | Method | Raises focus events. |

## Desired Integration Pattern

Rows expose a `Bind` method (or typed properties) so hosts never use `GetComponentInChildren`:

```csharp
using RO_Flex_UI.Components;
using RO_Flex_UI.Panels;
using UnityEngine;

public class InventoryRow : ListItem
{
    [SerializeField] private IconText content;

    public void Bind(InventoryEntry entry)
    {
        content.Sprite = entry.Icon;
        content.Text = entry.Name;
    }
}

// Host
listPanel.Clear();
listPanel.AddItems(entries, (row, entry) => ((InventoryRow)row).Bind(entry));
row.OnItemActivated.AddListener(() => host.OnRowChosen(row));
```

Focus and activation should also be observable at the panel level:

```csharp
// Desired — not yet available on ListPanel
listPanel.OnItemFocused.AddListener(host.OnRowFocused);
listPanel.OnItemActivated.AddListener(host.OnRowActivated);
```

Rows used outside a `ListPanel` should work through an `IListItemHost` callback instead of a hard `ListPanel` reference.

## Related Scenarios

- [Selectable list](../../integration-review.md#scenario-selectable-list)
- [Equipment gear display](../../integration-review.md#scenario-equipment-gear)

[Back to specs index](../README.md)
