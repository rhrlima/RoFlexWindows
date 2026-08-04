# IconAmount

[Go Back](../README.md)

## Description

`IconAmount` displays a sprite and an amount label. It can show or hide either
element and clear its contents. `DraggableManager` also uses an `IconAmount` as
the shared visual proxy for payload-based drag-and-drop.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| `Sprite` | `Sprite` | Sprite currently assigned to the icon image. |
| `Amount` | `string` | Amount label, or an empty string when unavailable. |
| `IsVisible` | `bool` | Whether the component is marked visible and at least one element is enabled. |
| `Empty` | `bool` | Whether both sprite and amount are empty. |

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Sprite` | `Sprite` | Current icon sprite. |
| `Amount` | `string` | Current amount text. |
| `IsVisible` | `bool` | Current logical visibility. |
| `Empty` | `bool` | Current content-empty state. |

### Methods

| Method | Description |
| --- | --- |
| `Assign(sprite, amount)` | Replaces the sprite and amount text. `null` amount becomes empty. |
| `SetActive(value)` | Shows or hides both visual elements and updates logical visibility. |
| `ToggleIcon(active)` | Shows or hides only the icon GameObject. |
| `ToggleAmount(active)` | Shows or hides only the amount GameObject. |
| `Clear()` | Removes both values and hides the component. |

## Examples

```csharp
slot.Assign(item.Icon, item.Quantity.ToString());
slot.SetActive(true);

if (item.Quantity == 0)
    slot.Clear();
```

## Related Components

- [IconText](icon-text.md)
- [Drag and Drop](drag-and-drop.md)
- [DraggableManager](draggable-manager.md)
- [FillPanel](../Panels/fill-panel.md)

[Go Back](../README.md)
