# DragDropTrigger

[Go Back](../README.md)

## Description

`DragDropTrigger` combines `DragTrigger` source handling with Unity's
`IDropHandler`. Use it for a presenter that implements both `IDragSource` and
`IDragTarget`.

It resolves both interfaces from its GameObject or parent hierarchy and
forwards drag and drop callbacks to `DraggableManager`.

## Implemented Interfaces

| Interface | Callback | Behavior |
| --- | --- | --- |
| `IBeginDragHandler` | `OnBeginDrag(PointerEventData eventData)` | Begins a manager session for the resolved source. |
| `IDragHandler` | `OnDrag(PointerEventData eventData)` | Forwards proxy movement for the resolved source. |
| `IEndDragHandler` | `OnEndDrag(PointerEventData eventData)` | Ends the manager session for the resolved source. |
| `IDropHandler` | `OnDrop(PointerEventData eventData)` | Attempts to deliver the active payload to the resolved target. |

Missing source or target interfaces are logged independently.

## Setup

1. Implement both `IDragSource` and `IDragTarget` on a UI presenter.
2. Add `DragDropTrigger` to the root or child `Graphic` that should receive
   pointer events.
3. Keep **Raycast Target** enabled on that `Graphic`.
4. Configure the scene's `DraggableManager` and shared proxy.

## Related Pages

- [Drag and Drop](drag-and-drop.md)
- [`DragTrigger`](drag-trigger.md)
- [`DropTrigger`](drop-trigger.md)
- [`IDragSource`](idrag-source.md)
- [`IDragTarget`](idrag-target.md)
- [`DraggableManager`](draggable-manager.md)

[Go Back](../README.md)
