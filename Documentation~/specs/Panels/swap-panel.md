# SwapPanel — Integration Spec

[Back to specs index](../README.md)

## Purpose

Shows one child panel group at a time from a configured set of groups. Used to swap between alternative panel layouts (e.g. list vs. grid view) without separate tab UI.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `activeGroup` | `int` | Currently active group id (public field). |
| `SwapByGroup(int)` / `SwapByIndex(int)` | Methods | Activate a group by id or ordered index. |
| `GetNextGroup()` / `GetPreviousGroup()` | Methods | Cycle to next/previous group in `groups` list. |
| `EnsureReferences()` | `bool` | **Throws `NotImplementedException`.** |

`onSwapEvent` is private serialized with no C# accessor.

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Panels;

swapPanel.SwapByGroup(InventoryView.Grid);
swapPanel.OnSwapped.AddListener(host.OnViewModeChanged); // desired

// Encapsulated active group — desired:
var current = swapPanel.ActiveGroup;
```

Rename `GetNextGroup` / `GetPreviousGroup` to `SwapToNextGroup` / `SwapToPreviousGroup` for clarity.

## Related Scenarios

- [Panel group swapping](../../integration-review.md#scenario-panel-swap)

[Back to specs index](../README.md)
