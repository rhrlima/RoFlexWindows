# Draggable / DraggableBase — Integration Spec

[Back to specs index](../README.md)

## Purpose

Moves a `RectTransform` with pointer drag inside a canvas. `Draggable` is an empty subclass of `DraggableBase`. Used by `Window` for title-bar dragging and any free-position UI element.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `onBeginDrag` / `onDrag` / `onEndDrag` | `UnityEvent<PointerEventData>` | Drag lifecycle events. |
| `ReturnToOrigin` | `bool` | Snaps back to start position on drag end. |
| `Dragging` | `bool` | True while a drag session is active. |
| `EnsureReferences()` | `bool` | Requires a parent `Canvas` and target `RectTransform`. |

`targetTransform` is serialized; defaults to the component's own `RectTransform`.

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Components;

draggable.ReturnToOrigin = false;
draggable.onEndDrag.AddListener(_ => host.OnPanelMoved());
```

Window z-order should be managed by a central window manager rather than per-component `SetAsLastSibling` calls. `Window.OnPointerDown` currently brings windows forward.

## Related Scenarios

- [Movable / resizable window](../../integration-review.md#scenario-window)

[Back to specs index](../README.md)
