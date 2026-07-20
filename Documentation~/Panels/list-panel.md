# ListPanel

[Go Back](../README.md)

## Description

`ListPanel` manages an ordered collection of selectable `ListItem` components.
Items can be assigned explicitly in the Inspector, supplied as populated
`ListItem` instances, or generated from a template and bound to application data.

All public addition methods append to the current list. Call `Clear()` before
adding items when replacing the displayed content.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| Items | `List<ListItem>` | Explicit Inspector references registered in their configured order during `Start`. |
| Focused item | `ListItem` | Item most recently focused by pointer or UI selection. |
| Activated item | `ListItem` | Item most recently submitted or double-clicked. |

## Configuration

| Field | Description |
| --- | --- |
| `viewport` | Visible scroll area used to keep the focused item in view. |
| `template` | Inactive `ListItem` cloned by the data-based `AddItems` overload. |
| `loopNavigation` | Connects the first and last items when navigating vertically. |
| `autoScroll` | Scrolls the focused item into the viewport. |
| `items` | Items registered from the Inspector when the panel initializes. Former `initialItems` and `listItems` data migrates to this field. |

Only entries referenced by `items` are registered automatically. Other
children of the panel are ignored.

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `FocusedItem` | `ListItem` | Currently focused item, or `null`. |
| `ActivatedItem` | `ListItem` | Most recently activated item, or `null`. |
| `LoopNavigation` | `bool` | Enables or disables navigation between the first and last items. |
| `AutoScroll` | `bool` | Enables or disables scrolling focused items into view. |

### Methods

| Method | Description |
| --- | --- |
| `AddItem(item)` | Appends one populated `ListItem`. |
| `AddItems(items)` | Appends populated `ListItem` instances in enumeration order. |
| `AddItems<TData>(data, bind, template)` | Clones a template for each data value and invokes `bind` for the new item. The configured template is used when the argument is omitted. |
| `Clear()` | Destroys all registered items and resets focus and activation state. |
| `SelectOption(index)` | Selects the item at a valid zero-based index. |
| `FitOptionToView(item)` | Scrolls an item into the viewport when auto-scroll is enabled. |
| `NotifyItemFocused(item)` | Updates the focused item. Called by `ListItem`. |
| `NotifyItemActivated(item)` | Updates the activated item. Called by `ListItem`. |

Supplied items must already contain `ListItem` and its required `RoButton`.
After registration, the panel reparents and owns them; `Clear()` destroys them.

## Examples

### Display Data

```csharp
using System.Collections.Generic;
using RO_Flex_UI.Components;
using RO_Flex_UI.Panels;
using TMPro;
using UnityEngine;

public class InventoryList : MonoBehaviour
{
    [SerializeField] private ListPanel listPanel;

    public void ShowItems(IEnumerable<string> itemNames)
    {
        listPanel.Clear();
        listPanel.AddItems(itemNames, BindItem);
    }

    private static void BindItem(ListItem item, string itemName)
    {
        item.GetComponentInChildren<TMP_Text>().text = itemName;
    }
}
```

### Add Populated Items

```csharp
listPanel.AddItem(primaryEntry);
listPanel.AddItems(additionalEntries);
listPanel.SelectOption(0);
```

### Use a Different Template

```csharp
listPanel.Clear();
listPanel.AddItems(itemNames, BindItem, compactItemTemplate);
```

## Related Components

- [ListItem](../Components/list-item.md)
- [RoButton](../Components/ro-button.md)
- [Scroll Panel](scroll-panel.md)

[Go Back](../README.md)
