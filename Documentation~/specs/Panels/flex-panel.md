# FlexPanel — Integration Spec

[Back to specs index](../README.md)

## Purpose

Applies fixed or proportional flex sizing to child `RectTransform` entries using `LayoutElement`. Used for split panes and proportional tool layouts.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `LayoutOrientation` | `Orientation` | `Vertical` or `Horizontal`; triggers `Apply()`. |

`entries` (fixed/flex sizes and proportions) are private and editor-driven. `Apply()` runs on `OnEnable` and `OnValidate`.

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Panels;

flexPanel.SetEntryProportion(0, 2f);  // desired
flexPanel.SetEntryFixedSize(1, 48f); // desired
flexPanel.LayoutOrientation = FlexPanel.Orientation.Horizontal;
flexPanel.Rebuild();                 // desired
```

Document that `Apply()` may add `LayoutElement` components at runtime.

[Back to specs index](../README.md)
