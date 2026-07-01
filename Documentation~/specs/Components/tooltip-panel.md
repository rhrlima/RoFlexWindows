# TooltipPanel — Integration Spec

[Back to specs index](../README.md)

## Purpose

Hover target that shows an associated `Tooltip`. Hides the tooltip during drag and when the sibling `IconAmount` is not visible.

## Current Public Interface

Implements `IPointerEnterHandler`, `IPointerExitHandler`, `IBeginDragHandler`. Serialized fields:

| Field | Description |
| --- | --- |
| `tooltipEnabled` | Master enable flag. |
| `tooltipComponent` | Reference to the shared `Tooltip` instance. |

No public methods. Tooltip text must be set on `Tooltip` directly before hover.

## Desired Integration Pattern

```csharp
// Desired — host sets content on the panel, not the global Tooltip
tooltipPanel.SetTooltipText(BuildItemDescription(item));
```

Or bind from sibling `IconAmount` content automatically when no custom text is set.

## Related Scenarios

- [Item tooltip on hover](../../integration-review.md#scenario-item-tooltip)

[Back to specs index](../README.md)
