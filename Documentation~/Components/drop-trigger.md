# DropTrigger

[Go Back](../README.md)

## Description

`DropTrigger` receives Unity UI drop callbacks from a raycastable `Graphic` and
forwards them to `DraggableManager`.

It resolves an `IDragTarget` from its GameObject or parent hierarchy. Use it for
a target-only presenter. If an `IDragSource` is also present in the hierarchy,
the trigger forwards it so the manager can reject self-drops.

## Implemented Interface

| Interface | Callback | Behavior |
| --- | --- | --- |
| `IDropHandler` | `OnDrop(PointerEventData eventData)` | Attempts to deliver the active payload to the resolved target. |

If no target is found, the trigger logs an error and ignores drop callbacks.

## Setup

1. Implement `IDragTarget` on a UI presenter.
2. Add `DropTrigger` to the root or child `Graphic` that should receive drop
   events.
3. Keep **Raycast Target** enabled on that `Graphic`.
4. Configure the scene's `DraggableManager` and shared proxy.

## Related Pages

- [Drag and Drop](drag-and-drop.md)
- [`DragTrigger`](drag-trigger.md)
- [`DragDropTrigger`](drag-drop-trigger.md)
- [`IDragTarget`](idrag-target.md)
- [`DraggableManager`](draggable-manager.md)

[Go Back](../README.md)
