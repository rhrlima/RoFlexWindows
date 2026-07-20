# Resize Corner

[Go Back](../README.md)

## Description

`Resizable` is the behavior used by the Resize Corner prefab. Dragging the
corner resizes an assigned `RectTransform`, clamps it to minimum and maximum
sizes, and can snap the result to a configurable step.

## Displayed Data

`Resizable` displays no data. It changes the target transform's `sizeDelta` and,
unless `ignoreAnchor` is enabled, adjusts its anchored position while resizing.

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `MinSize` | `Vector2` | Minimum allowed target size. |
| `MaxSize` | `Vector2` | Maximum allowed target size. |
| `StepSize` | `Vector2` | Snap increment used when step snapping is enabled. |

### Events

| Event | Description |
| --- | --- |
| `onBeginResize` | Invoked when a resize drag begins. |
| `onResize` | Invoked while the left pointer resizes the target. |
| `onEndResize` | Invoked when the resize drag ends. |

## Examples

```csharp
resizeCorner.MinSize = new Vector2(160, 120);
resizeCorner.MaxSize = new Vector2(800, 600);
resizeCorner.onEndResize.AddListener(SaveWindowSize);
```

[Go Back](../README.md)
