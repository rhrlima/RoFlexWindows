# Drag and Drop

[Go Back](../README.md)

## Description

The package drag-and-drop API transfers a `DragPayload` from an `IDragSource`
to an `IDragTarget` while displaying a shared `IconAmount` proxy beneath a
Canvas.

## Runtime Scripts

| Script | Responsibility |
| --- | --- |
| [`DragPayload`](drag-payload.md) | Carries the source, project data, sprite, and text. |
| [`IDragSource`](idrag-source.md) | Defines whether and how a payload can be dragged. |
| [`IDragTarget`](idrag-target.md) | Defines whether and how a payload can be accepted. |
| [`DraggableManager`](draggable-manager.md) | Owns the active session, payload, and visual proxy. |
| [`DragTrigger`](drag-trigger.md) | Forwards source-only drag callbacks to the manager. |
| [`DropTrigger`](drop-trigger.md) | Forwards target-only drop callbacks to the manager. |
| [`DragDropTrigger`](drag-drop-trigger.md) | Combines source and target callback handling. |

This API is separate from [`Draggable`](draggable.md), which directly moves a
`RectTransform`.

## Session Flow

1. `DragTrigger.OnBeginDrag` forwards the resolved source and pointer data.
2. `DraggableManager` validates the source, creates the payload, displays the
   proxy, and records session ownership.
3. Drag callbacks move the proxy using pointer delta and the Canvas scale
   factor.
4. A target trigger asks the manager to validate and complete the drop.
5. For an accepted drop, the manager calls
   `IDragTarget.OnDropComplete(payload)` followed by
   `IDragSource.OnDragComplete()`.
6. The source end-drag callback clears the proxy, payload, and active source.

Ending a drag clears the session whether or not a target accepted the payload.

## Basic Setup

1. Add and configure one `DraggableManager` beneath the relevant Canvas.
2. Assign an `IconAmount` proxy to the manager.
3. Implement `IDragSource`, `IDragTarget`, or both on the UI presenter.
4. Add `DragTrigger`, `DropTrigger`, or `DragDropTrigger` to the raycastable
   `Graphic`, according to the presenter's roles.

## Related Pages

- [`DragPayload`](drag-payload.md)
- [`IDragSource`](idrag-source.md)
- [`IDragTarget`](idrag-target.md)
- [`DraggableManager`](draggable-manager.md)
- [`DragTrigger`](drag-trigger.md)
- [`DropTrigger`](drop-trigger.md)
- [`DragDropTrigger`](drag-drop-trigger.md)
- [`Draggable`](draggable.md)

[Go Back](../README.md)
