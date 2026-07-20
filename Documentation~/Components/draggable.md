# Draggable

[Go Back](../README.md)

## Description

`Draggable` is the package's general-purpose UI movement component. It inherits
all behavior from `DraggableBase`: a left-pointer drag moves an assigned
`RectTransform` by the pointer delta adjusted for the parent canvas scale.

The target defaults to the component's own `RectTransform`. The component must
be placed under a `Canvas`.

## Displayed Data

`Draggable` displays no data. It changes the target transform's anchored
position while a drag is active.

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `ReturnToOrigin` | `bool` | Restores the target's drag-start position when the drag ends. |
| `Dragging` | `bool` | Whether a valid left-pointer drag is active. |

### Methods

| Method | Description |
| --- | --- |
| `EnsureReferences()` | Resolves the parent canvas and default target, returning whether both are valid. |
| `OnBeginDrag(eventData)` | Stores the origin and begins a left-pointer drag. |
| `OnDrag(eventData)` | Moves the target while dragging. |
| `OnEndDrag(eventData)` | Ends the drag and optionally restores the origin. |

### Events

| Event | Description |
| --- | --- |
| `onBeginDrag` | Invoked after a valid drag begins. |
| `onDrag` | Invoked after each active drag movement. |
| `onEndDrag` | Invoked when an active drag ends. |

Disabling the component ends an active drag. The component does not implement
bounds, inertia, snapping, or automatic layout integration.

## Examples

```csharp
[SerializeField] private Draggable windowDrag;

private void Awake()
{
    windowDrag.ReturnToOrigin = false;
    windowDrag.onEndDrag.AddListener(_ => SaveWindowPosition());
}
```

## Related Components

- [DraggableItem](draggable-item.md)
- [Resize Corner](resize-corner.md)

[Go Back](../README.md)
