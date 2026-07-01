# TabsPanel — Integration Spec

[Back to specs index](../README.md)

## Purpose

Tabbed navigation pairing `TabButton` instances with content panel GameObjects. Invokes per-tab enter/exit events on selection change.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `CurrentIndex` | `int` | Active tab index, or `-1`. |
| `SetActiveTab(int)` | Method | Selects a tab by index. |

Per-entry `onPanelEnter` / `onPanelExit` are Inspector-only. No runtime tab label API, dynamic registration, or panel-level unified event.

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Panels;

tabsPanel.SetTabLabel(0, "Inventory");  // desired
tabsPanel.SetActiveTab(1);
tabsPanel.OnTabChanged.AddListener(host.OnTabChanged); // desired
```

Runtime-added tabs need `RegisterTab(TabButton, GameObject)` so click listeners wire correctly (today listeners register only in `Start`).

See also [TabsPanel reference doc](../../Panels/tabs-panel.md).

## Related Scenarios

- [Tabbed panel host](../../integration-review.md#scenario-tabbed-panel)

[Back to specs index](../README.md)
