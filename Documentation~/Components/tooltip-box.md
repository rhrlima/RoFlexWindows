# TooltipBox

[Go Back](../README.md)

## Description

`TooltipBox` displays tooltip text in the Tooltip Proxy prefab. It follows the
mouse, sizes its background to the text's preferred dimensions, and clamps that
size to the configured maximum.

The component requires a `CanvasGroup`, a text reference, and a background
`RectTransform`. It disables raycast blocking during initialization.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| Text | `TMP_Text` | Current tooltip content. |
| Background size | `Vector2` | Preferred text size clamped by `maxSize`. |

## Public API

### Methods

| Method | Description |
| --- | --- |
| `EnsureReferences()` | Validates the canvas group, text, and background references. |
| `SetText(text)` | Replaces the displayed text and refreshes the background size. |
| `ShowTooltip()` | Refreshes the size and activates the GameObject. |
| `HideTooltip()` | Deactivates the GameObject. |

## Examples

```csharp
tooltipBox.SetText(item.Description);
tooltipBox.ShowTooltip();
```

Use `TooltipTrigger` when pointer enter, exit, and drag events should control the
box automatically.

## Related Components

- [TooltipTrigger](tooltip-trigger.md)

[Go Back](../README.md)
