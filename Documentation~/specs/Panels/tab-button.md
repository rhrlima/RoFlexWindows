# TabButton — Integration Spec

[Back to specs index](../README.md)

## Purpose

Tab header button that swaps between active and idle sprites. Selected by `TabsPanel` via `SetActive(bool)`.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `SetActive(bool)` | Method | Applies active or idle sprite (not `GameObject` active state). |
| `EnsureReferences()` | `bool` | Validates `Image` reference. |

Inherits `RoButton` / `Button` (`onClick`, `interactable`, etc.). No label API.

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Panels;

tabButton.Label = "Skills";     // desired
tabsPanel.SetActiveTab(index);  // panel calls tabButton.SetSelected(true) — desired rename
```

`SetActive` should be renamed to `SetSelected` to avoid confusion with `GameObject.SetActive`.

## Related Scenarios

- [Tabbed panel host](../../integration-review.md#scenario-tabbed-panel)

[Back to specs index](../README.md)
