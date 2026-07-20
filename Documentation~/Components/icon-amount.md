# IconAmount

[Go Back](../README.md)

## Description

`IconAmount` displays a sprite and an amount label. It can show or hide either
element, clear its contents, and participate in drag-and-drop presentation
through `IDragVisual`.

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
| `RectTransform` | `RectTransform` | This component's UI transform. |
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
| `CapturePresentation()` | Captures the sprite and amount as a `DragPresentation`. |
| `TryApplyPresentation(presentation)` | Applies the presentation fields supported by this component. |

## Examples

```csharp
slot.Assign(item.Icon, item.Quantity.ToString());
slot.SetActive(true);

if (item.Quantity == 0)
    slot.Clear();
```

## Related Components

- [IconText](icon-text.md)
- [DraggableItem](draggable-item.md)
- [DropZone](drop-zone.md)
- [FillPanel](../Panels/fill-panel.md)

[Go Back](../README.md)
