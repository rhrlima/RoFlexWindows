# Draggable

[Go Back](../README.md)

## Description

`Draggable` moves a `RectTransform` in response to Unity UI drag events. Use it
for windows, panels, and other UI elements that should follow the pointer. It
does not transfer data between UI elements; use the
[package drag-and-drop system](drag-and-drop.md) for payload-based interactions.

The component accepts left-button drags, applies pointer delta using the parent
Canvas scale factor, and can optionally restore the original anchored position
when the drag ends.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| `targetTransform` | `RectTransform` | Transform moved during a drag. Defaults to the component's own `RectTransform`. |
| `returnToOrigin` | `bool` | Restores the position captured at the start of the drag when the drag ends. |

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `ReturnToOrigin` | `bool` | Gets or changes whether the target returns to its starting position. |
| `Dragging` | `bool` | Reports whether this component currently owns an active drag. |

### Methods

| Method | Description |
| --- | --- |
| `EnsureReferences()` | Resolves the parent `Canvas` and default target, returning whether the component is ready. |
| `OnBeginDrag(PointerEventData eventData)` | Starts a left-button drag, disables the drag threshold, and captures the target position. |
| `OnDrag(PointerEventData eventData)` | Moves the target by the pointer delta divided by the Canvas scale factor. |
| `OnEndDrag(PointerEventData eventData)` | Ends the drag and optionally restores the captured position. |

### Events

| Event | Description |
| --- | --- |
| `onBeginDrag` | Raised after a valid left-button drag begins. |
| `onDrag` | Raised after the target moves during an active drag. |
| `onEndDrag` | Raised when an active drag ends. |

## Examples

### Inspector Setup

1. Add `Draggable` to a UI object beneath a `Canvas`.
2. Leave **Target Transform** empty to move the same object, or assign another
   `RectTransform`.
3. Enable **Return To Origin** if the object should snap back on release.
4. Add listeners to the drag events only when the surrounding UI needs
   notifications.

The GameObject receiving pointer events must be raycastable. In a Canvas, this
normally means it has a `Graphic` with **Raycast Target** enabled and is not
covered by another raycastable element.

### Configure From Code

```csharp
using RO_Flex_UI.Components;
using UnityEngine;

public sealed class DragSetup : MonoBehaviour
{
    [SerializeField] private Draggable draggable;

    private void Awake()
    {
        draggable.ReturnToOrigin = true;
    }
}
```

## Troubleshooting

- If dragging does not start, confirm the object is below a `Canvas` and can
  receive UI raycasts.
- If another transform should move, assign **Target Transform** explicitly.
- If a layout group resets the position, move an object outside the managed
  layout or update the layout strategy.
- Only left-button pointer events start a drag.

## Related Pages

- [Drag and Drop](drag-and-drop.md)
- [`DraggableManager`](draggable-manager.md)
- [`DragTrigger`](drag-trigger.md)
- [`DropTrigger`](drop-trigger.md)
- [`DragDropTrigger`](drag-drop-trigger.md)

[Go Back](../README.md)
