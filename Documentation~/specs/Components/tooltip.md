# Tooltip — Integration Spec

[Back to specs index](../README.md)

## Purpose

Floating text tooltip that follows the pointer and sizes its background to content. Typically referenced by `TooltipPanel` on hover targets.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `SetText(string)` | Method | Sets tooltip body and refreshes size. |
| `ShowTooltip()` / `HideTooltip()` | Method | Shows or hides the tooltip GameObject. |
| `EnsureReferences()` | `bool` | Validates `CanvasGroup`, text, and background. |

Lives in the global namespace (not `RO_Flex_UI.Components`). `Update` positions via `Input.mousePosition` without canvas scaling.

## Desired Integration Pattern

```csharp
tooltip.SetText("Iron Sword\nATK +12");
tooltip.ShowTooltip();
// ...
tooltip.HideTooltip();
```

Position should convert through the owning canvas for scaled UI and multi-display setups. Public methods should guard when references are missing.

## Related Scenarios

- [Item tooltip on hover](../../integration-review.md#scenario-item-tooltip)

[Back to specs index](../README.md)
