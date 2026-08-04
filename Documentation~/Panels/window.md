# Window

[Go Back](../README.md)

## Description

`Window` is the base behavior for package windows. It controls visibility,
optional dragging and resizing, sibling order, initial centering, and whether
the window remains inside its parent play area.

The component discovers child [`Draggable`](../Components/draggable.md) and
[`Resizable`](../Components/resize-corner.md) behaviors. Missing optional
behaviors are ignored.

## Configuration

| Field | Description |
| --- | --- |
| **Reset To Center** | Centers the window whenever it is enabled. |
| **Is Draggable** | Enables the discovered child `Draggable`. |
| **Is Resizable** | Activates and enables the discovered child `Resizable`. |
| **Keep Window In Screen** | Clamps the window to its parent `RectTransform` each frame. |
| **Return To Origin** | Configures the child `Draggable` to restore its drag-start position on release. |

The immediate parent must be a `RectTransform` for centering and play-area
clamping.

## Public API

### Methods

| Method | Description |
| --- | --- |
| `ShowWindow()` | Brings the window to the front and activates it. |
| `HideWindow()` | Deactivates the window. |
| `ToggleVisibility()` | Shows or hides the window according to its current hierarchy state. |
| `ToggleDraggable()` | Synchronizes the child `Draggable` enabled state with **Is Draggable**. |
| `ToggleResisable()` | Synchronizes the child `Resizable` active and enabled states with **Is Resizable**. |
| `CenterWindow()` | Positions the window at the center of its immediate parent while accounting for anchors and pivot. |
| `FitWindowIntoPlayArea()` | Clamps the window inside its immediate parent when **Keep Window In Screen** is enabled. |
| `OnPointerDown(PointerEventData eventData)` | Brings the window to the front for left- or right-button presses. |

`ToggleResisable` retains the spelling used by the current public API.

## Examples

```csharp
[SerializeField] private Window inventoryWindow;

public void ToggleInventory()
{
    inventoryWindow.ToggleVisibility();
}
```

## Related Pages

- [Draggable](../Components/draggable.md)
- [Resize Corner](../Components/resize-corner.md)
- [Header](../Components/header.md)

[Go Back](../README.md)
