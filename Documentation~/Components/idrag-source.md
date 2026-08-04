# IDragSource

[Go Back](../README.md)

## Description

`IDragSource` defines the package contract for a component that can begin a
proxy drag-and-drop session. The source decides whether dragging is currently
allowed, creates the payload, and responds after an accepted drop.

## Public API

### Methods

| Method | Description |
| --- | --- |
| `CanDrag()` | Returns whether `DraggableManager` may begin a drag from this source. |
| `CreatePayload()` | Creates the `DragPayload` for the new session. The manager rejects a null result. |
| `OnDragComplete()` | Called after an `IDragTarget` accepts and applies the payload. |

`OnDragComplete()` is not called when the drag ends without an accepted drop.
The implementation decides whether accepted drops move, copy, or otherwise
update the source data.

`DragTrigger` and `DragDropTrigger` search their GameObject and parent
hierarchy for this interface. A presenter may implement `IDragSource` alone or
together with `IDragTarget`.

## Related Pages

- [Drag and Drop](drag-and-drop.md)
- [`DragPayload`](drag-payload.md)
- [`IDragTarget`](idrag-target.md)
- [`DraggableManager`](draggable-manager.md)
- [`DragTrigger`](drag-trigger.md)
- [`DragDropTrigger`](drag-drop-trigger.md)

[Go Back](../README.md)
