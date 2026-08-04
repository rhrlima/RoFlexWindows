# DragTrigger

[Go Back](../README.md)

## Description

`DragTrigger` receives Unity UI drag callbacks from a raycastable `Graphic` and
forwards them to `DraggableManager`.

It resolves an `IDragSource` from its GameObject or parent hierarchy. Use it for
a source-only presenter.

## Implemented Interfaces

| Interface | Callback | Behavior |
| --- | --- | --- |
| `IBeginDragHandler` | `OnBeginDrag(PointerEventData eventData)` | Begins a manager session for the resolved source. |
| `IDragHandler` | `OnDrag(PointerEventData eventData)` | Forwards proxy movement for the resolved source. |
| `IEndDragHandler` | `OnEndDrag(PointerEventData eventData)` | Ends the manager session for the resolved source. |

If no source is found, the trigger logs an error and ignores drag callbacks.

## Setup

1. Implement `IDragSource` on a UI presenter.
2. Add `DragTrigger` to the root or child `Graphic` that should receive pointer
   events.
3. Keep **Raycast Target** enabled on that `Graphic`.
4. Configure the scene's `DraggableManager` and shared proxy.

## Related Pages

- [Drag and Drop](drag-and-drop.md)
- [`DropTrigger`](drop-trigger.md)
- [`DragDropTrigger`](drag-drop-trigger.md)
- [`IDragSource`](idrag-source.md)
- [`DraggableManager`](draggable-manager.md)

[Go Back](../README.md)
