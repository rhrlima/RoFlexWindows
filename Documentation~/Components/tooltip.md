# Tooltip Trigger and Proxy

[Go Back](../README.md)

## Description

`TooltipTrigger` shows a referenced `TooltipBox` while the pointer is over its
target. The Tooltip Proxy prefab supplies the box, which sizes its background to
the assigned text up to its configured maximum. Tooltips hide on pointer exit,
drag start, or trigger disable.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| `TooltipText` | `string` | Text assigned to the trigger. |
| `Enabled` | `bool` | Whether pointer entry may show the tooltip. |

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Enabled` | `bool` | Enables or disables tooltip display. |
| `TooltipText` | `string` | Current trigger text. |
| `OnTrigger` | `Action` | Callback invoked on pointer entry before visibility is evaluated. |

### Methods

| Method | Description |
| --- | --- |
| `TooltipTrigger.SetText(text)` | Replaces the text used on the next pointer entry. |
| `TooltipBox.SetText(text)` | Replaces the box text and refreshes its size. |
| `TooltipBox.ShowTooltip()` | Refreshes size and activates the box. |
| `TooltipBox.HideTooltip()` | Deactivates the box. |

## Examples

```csharp
tooltipTrigger.SetText(item.Description);
tooltipTrigger.Enabled = !string.IsNullOrEmpty(item.Description);
tooltipTrigger.OnTrigger += RefreshLiveDescription;
```

## Related Components

- [Drag and Drop](drag-and-drop.md)
- [TooltipTrigger](tooltip-trigger.md)
- [TooltipBox](tooltip-box.md)

[Go Back](../README.md)
