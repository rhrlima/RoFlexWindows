# Resizable — Integration Spec

[Back to specs index](../README.md)

## Purpose

Drag handle that resizes a target `RectTransform` with optional min/max bounds and step snapping. Used on window edges via `Window`.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `onBeginResize` / `onResize` / `onEndResize` | `ResizeEvent` | Resize lifecycle. |
| `MinSize` / `MaxSize` / `StepSize` | `Vector2` | Size constraints (`StepSize` setter bypasses `[Min(1f)]`). |
| `EnsureReferences()` | `bool` | Validates `targetTransform`. |

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Components;
using UnityEngine;

resizeHandle.MinSize = new Vector2(200, 150);
resizeHandle.MaxSize = new Vector2(800, 600);
resizeHandle.onEndResize.AddListener(_ => host.SaveWindowSize());
```

`OnBeginDrag` should apply the same left-button filter as `OnDrag`. `StepSize` should clamp to valid values in the setter.

## Related Scenarios

- [Movable / resizable window](../../integration-review.md#scenario-window)

[Back to specs index](../README.md)
