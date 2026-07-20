# RoSlider

[Go Back](../README.md)

## Description

`RoSlider` extends Unity's `Slider` with decrease and increase buttons,
direction-aware button sprites, percentage-based stepping, and a restricted
drag area. Pointer drags that start outside the configured area are ignored.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| `value` | `float` | Inherited current slider value. |

## Public API

### Events

| Event | Description |
| --- | --- |
| `onDecreaseClick` | Invoked with the updated value after the decrease button is clicked. |
| `onIncreaseClick` | Invoked with the updated value after the increase button is clicked. |
| `onPointerUp` | Invoked with the final value after a valid drag ends. |
| `onValueChanged` | Inherited slider event invoked when the value changes. |

The step is `(maxValue - minValue) * stepPerc`; whole-number sliders round that
step to an integer. Inherited min/max, direction, interactable, and value APIs
remain available.

## Examples

```csharp
slider.minValue = 0;
slider.maxValue = 100;
slider.value = 50;
slider.onPointerUp.AddListener(SaveVolume);
```

## Related Components

- [RoButton](ro-button.md)

[Go Back](../README.md)
