# ListPanel — Integration Spec

[Back to specs index](../README.md)

## Purpose

Manages an ordered, navigable list of `ListItem` rows with optional auto-scroll and loop navigation. Supports template-based data binding.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `FocusedItem` / `ActivatedItem` | `ListItem` | Last focused or activated row. |
| `LoopNavigation` / `AutoScroll` | `bool` | Navigation and scroll behavior. |
| `AddItem` / `AddItems` / `AddItems<T>(data, bind, template)` | Methods | Append rows. |
| `Clear()` | Method | Destroys all registered items. |
| `SelectOption(int)` | Method | Programmatically focus a row by index. |
| `FitOptionToView(ListItem)` | Method | Scroll focused row into viewport. |
| `NotifyItemFocused` / `NotifyItemActivated` | Methods | Called by `ListItem`. |
| `EnsureReferences()` | `bool` | Validates content transform and deactivates template. |

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Components;
using RO_Flex_UI.Panels;

listPanel.Clear();
listPanel.AddItems(data, (row, item) => row.Bind(item));
listPanel.OnItemActivated.AddListener(row => host.OnRowChosen(row)); // desired
listPanel.SelectOption(0);
```

Hosts need read access (`Count`, `GetItem(int)`, `IndexOf`) and panel-level focus/activate events. `Clear()` ownership should be documented; optional `Clear(destroyItems: bool)` for pooled rows.

See also [ListPanel reference doc](../../Panels/list-panel.md).

## Related Scenarios

- [Selectable list](../../integration-review.md#scenario-selectable-list)
- [Equipment gear display](../../integration-review.md#scenario-equipment-gear)

[Back to specs index](../README.md)
