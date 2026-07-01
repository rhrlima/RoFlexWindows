# Window — Integration Spec

[Back to specs index](../README.md)

## Purpose

Top-level UI window with optional centering, dragging, resizing, and screen clamping. Implements `IWindow` for show/hide/toggle.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `ShowWindow()` / `HideWindow()` / `ToggleVisibility()` | Methods | Visibility control. |
| `CenterWindow()` / `FitWindowIntoPlayArea()` | Methods | Positioning helpers. |
| `ToggleDraggable()` / `ToggleResisable()` | Methods | Sync child `Draggable` / `Resizable` with serialized flags. |
| `OnPointerDown` | Handler | Brings window to front on click. |

Flags (`isDraggable`, `isResizable`, `resetToCenter`, `keepWindowInScreen`, `returnToOrigin`) are serialize-only with no runtime properties.

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Panels;

window.IsDraggable = allowMove;    // desired
window.IsResizable = allowResize;  // desired
window.ShowWindow();
window.OnClosed.AddListener(host.OnWindowClosed); // desired — needs window manager / ESC
```

Screen clamping should be event-driven (drag/resize end) rather than every `LateUpdate` frame when many windows are open.

## Related Scenarios

- [Movable / resizable window](../../integration-review.md#scenario-window)

[Back to specs index](../README.md)
