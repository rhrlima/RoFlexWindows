# IDragTarget

[Go Back](../README.md)

## Description

`IDragTarget` defines the package contract for a component that can accept an
active `DragPayload`. The target validates the payload before applying it.

## Public API

### Methods

| Method | Description |
| --- | --- |
| `CanDrop(DragPayload payload)` | Returns whether this target accepts the active payload. |
| `OnDropComplete(DragPayload payload)` | Applies a payload after `CanDrop` succeeds. |

`DraggableManager` calls `OnDropComplete(payload)` before calling
`payload.source.OnDragComplete()`. Neither method is called for a rejected or
self-targeted drop.

`DropTrigger` and `DragDropTrigger` search their GameObject and parent
hierarchy for this interface. A presenter may implement `IDragTarget` alone or
together with `IDragSource`.

## Related Pages

- [Drag and Drop](drag-and-drop.md)
- [`DragPayload`](drag-payload.md)
- [`IDragSource`](idrag-source.md)
- [`DraggableManager`](draggable-manager.md)
- [`DropTrigger`](drop-trigger.md)
- [`DragDropTrigger`](drag-drop-trigger.md)

[Go Back](../README.md)
