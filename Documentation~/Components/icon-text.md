# IconText

[Go Back](../README.md)

## Description

`IconText` extends [IconAmount](icon-amount.md) with a text label and a required
horizontal or vertical layout group. It can also reverse the layout order.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| `Sprite` | `Sprite` | Inherited icon sprite. |
| `Amount` | `string` | Inherited amount label. |
| `Text` | `string` | Additional text label. |

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Text` | `string` | Current text label. |
| `IsVisible` | `bool` | Whether the component is visible and at least one element is enabled. |

### Methods

| Method | Description |
| --- | --- |
| `Assign(sprite, text, amount = "")` | Replaces all displayed values. |
| `FlipElements(flip)` | Sets the layout group's `reverseArrangement` value. |
| `ToggleText(active)` | Shows or hides the text GameObject. |
| `SetActive(value)` | Shows or hides the icon, amount, and text elements. |
| `Clear()` | Clears all three displayed values and hides the component. |

## Examples

```csharp
entry.Assign(item.Icon, item.DisplayName, item.Quantity.ToString());
entry.FlipElements(showIconOnRight);
```

## Related Components

- [IconAmount](icon-amount.md)
- [Drag and Drop](drag-and-drop.md)
- [GearPanel](../Panels/gear-panel.md)

[Go Back](../README.md)
