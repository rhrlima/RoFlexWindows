# DraggableManager

[Go Back](../README.md)

## Description

`DraggableManager` is the singleton coordinator for proxy drag-and-drop. It
owns the active source and payload, displays and moves the shared proxy, and
validates drop completion.

Only one session can be active. Events from a source that does not own the
session cannot move or end it.

## Serialized Fields

| Field | Type | Description |
| --- | --- | --- |
| `canvas` | `Canvas` | Coordinate space used to position and move the proxy. If omitted, the manager searches its parents. |
| `proxy` | `IconAmount` | Shared visual displayed while dragging. The manager makes its child graphics non-raycastable during initialization. |

Add and configure one manager beneath the Canvas used by the interaction. An
automatically created singleton has no Canvas or proxy references.

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Dragging` | `bool` | Reports whether a drag session is active. |

### Methods

| Method | Description |
| --- | --- |
| `TryGetPayload(out DragPayload payload)` | Returns the payload only during an active session. |
| `BeginDragSession(IDragSource source, PointerEventData eventData)` | Begins a left-button session when no session exists, the source allows dragging, and it creates a non-null payload. |
| `OnDrag(IDragSource source, PointerEventData eventData)` | Moves the proxy when `source` owns the active session. |
| `OnEndDragSession(IDragSource source, PointerEventData eventData)` | Clears the proxy, payload, and source when `source` owns the session. |
| `TryDrop(IDragSource targetSource, IDragTarget target)` | Rejects unavailable, invalid, and self-targeted drops; otherwise completes the target and then the payload source. |

## Drop Behavior

For an accepted drop, `TryDrop` calls:

1. `IDragTarget.CanDrop(payload)`
2. `IDragTarget.OnDropComplete(payload)`
3. `payload.source.OnDragComplete()`

The payload remains available until the owning source forwards its end-drag
event.

## Related Pages

- [Drag and Drop](drag-and-drop.md)
- [`DragPayload`](drag-payload.md)
- [`IDragSource`](idrag-source.md)
- [`IDragTarget`](idrag-target.md)
- [`DragTrigger`](drag-trigger.md)
- [`DropTrigger`](drop-trigger.md)
- [`DragDropTrigger`](drag-drop-trigger.md)

[Go Back](../README.md)
