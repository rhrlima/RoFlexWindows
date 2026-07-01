# RoSlider — Integration Spec

[Back to specs index](../README.md)

## Purpose

Slider with dedicated decrease/increase buttons and a constrained drag area. Used for volume, zoom, and other stepped value controls.

## Current Public Interface

Inherits `Slider` (`value`, `minValue`, `maxValue`, `onValueChanged`, etc.) plus:

| Member | Type | Description |
| --- | --- | --- |
| `onDecreaseClick` | `SliderEvent` | Invoked after decrease button adjusts value. |
| `onIncreaseClick` | `SliderEvent` | Invoked after increase button adjusts value. |
| `onPointerUp` | `SliderEvent` | Invoked when drag ends inside the drag area. |
| `EnsureReferences()` | `bool` | Validates decrease/increase buttons and drag area. |

## Desired Integration Pattern

Prefer `onValueChanged` for continuous updates; use button/pointer-up events when the host only commits on release:

```csharp
using RO_Flex_UI.Components;

volumeSlider.SetValueWithoutNotify(settings.Volume);
volumeSlider.onValueChanged.AddListener(host.OnVolumeChanged);
```

If `EnsureReferences()` fails, the component should disable itself so pointer handlers never run against null references.

[Back to specs index](../README.md)
