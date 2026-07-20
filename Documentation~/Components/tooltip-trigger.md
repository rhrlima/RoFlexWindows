# TooltipTrigger

[Go Back](../README.md)

## Description

`TooltipTrigger` shows a referenced [TooltipBox](tooltip-box.md) while the pointer
is over its GameObject. It hides the box on pointer exit, drag start, or component
disable.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| `TooltipText` | `string` | Text sent to the box on pointer entry. |
| `Enabled` | `bool` | Whether pointer entry may show the box. |

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Enabled` | `bool` | Enables or disables tooltip display. |
| `TooltipText` | `string` | Current tooltip text. |
| `OnTrigger` | `Action` | Callback invoked on pointer entry before visibility is evaluated. |

### Methods

| Method | Description |
| --- | --- |
| `SetText(text)` | Replaces the text used on the next pointer entry. |
| `OnPointerEnter(eventData)` | Invokes `OnTrigger`, then updates and shows the box when allowed. |
| `OnPointerExit(eventData)` | Hides the box. |
| `OnBeginDrag(eventData)` | Hides the box when dragging begins. |

## Examples

```csharp
tooltipTrigger.SetText(item.Description);
tooltipTrigger.Enabled = !string.IsNullOrEmpty(item.Description);
tooltipTrigger.OnTrigger += RefreshLiveDescription;
```

## Related Components

- [TooltipBox](tooltip-box.md)
- [DraggableItem](draggable-item.md)

[Go Back](../README.md)
