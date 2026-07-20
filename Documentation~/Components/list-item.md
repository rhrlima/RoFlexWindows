# ListItem

[Go Back](../README.md)

## Description

`ListItem` turns a `RoButton` into an entry managed by `ListPanel`. Pointer
hover or UI selection focuses the item. Submit or two left-clicks within the
configured interval activate it.

## Displayed Data

`ListItem` does not own label data. Add text, icons, and other visuals as child
components and populate them before or during `ListPanel.AddItems`.

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `TargetButton` | `RoButton` | Required button used for selection and navigation. |

### Methods

| Method | Description |
| --- | --- |
| `BindToPanel(panel)` | Assigns the panel notified by focus and activation. |
| `FocusItem()` | Selects the button and notifies the bound panel. |
| `EnsureReferences()` | Finds and validates the required `RoButton`. |

### Events

| Event | Description |
| --- | --- |
| `OnItemFocused` | Invoked when the item gains pointer or selection focus. |
| `OnItemActivated` | Invoked on submit or a recognized double-click. |

## Examples

```csharp
listItem.OnItemActivated.AddListener(OpenSelectedItem);
listPanel.AddItem(listItem);
```

## Related Components

- [ListPanel](../Panels/list-panel.md)
- [RoButton](ro-button.md)

[Go Back](../README.md)
