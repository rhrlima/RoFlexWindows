# Component: Draggable

[Go Back](../README.md)

## Overview

The `Draggable` component enables UI elements and game objects to be moved by the user with click-and-drag or touch input. It is designed for flexible drag interactions in the RO Flex UI system and works with common panel and container layouts.

### Key Concepts

- **Drag target**: The object that is moved by the user's pointer input.
- **Drag bounds**: Optional limits that restrict movement to a specific area.
- **Release behavior**: How the object behaves when dragging stops (snap, inertia, reset).

## Prerequisites

Before using `Draggable`, make sure you have:

- [ ] A UI or scene object with a `RectTransform` or `Transform`
- [ ] A `Canvas` set up for UI elements, if used in UI context
- [ ] A pointer input system configured (mouse, touch, or UI input)

## Step-by-Step Instructions

### Task 1: Add the Draggable Component

1. Select the object you want to make draggable.
2. Attach the `Draggable` component in the Inspector.
3. Configure the drag settings:
   - Set the drag mode or input behavior
   - Enable bounds if movement should be constrained
   - Assign any required target or handle references

**Expected Result**: The object is now responsive to drag input.

---

### Task 2: Configure Drag Constraints

1. Enable bounds or container constraints on the `Draggable` component.
2. Set the allowed area using a `RectTransform` or defined boundary values.
3. Test the drag behavior to confirm the object stays within the allowed range.

**Expected Result**: The object moves only within the configured bounds.

---

### Task 3: Set Release Behavior

1. Choose the release mode for the component (snap back, hold position, or inertia).
2. If snapping is enabled, specify the target position or anchor point.
3. Adjust any smoothing or damping settings for a polished interaction.

**Expected Result**: The object behaves predictably after the user stops dragging.

## Tips & Tricks

### Tip 1: Use a Drag Handle

If you only want part of the object to be draggable, attach a separate handle object and assign it in the component settings.

### Tip 2: Combine with Layout Groups

When using `Draggable` inside layout-managed containers, verify that the parent layout does not fight the drag movement.

### Tip 3: Test Touch Input

Verify the drag behavior on the target platform, especially if the object should work with touch or multitouch gestures.

## Troubleshooting

### Problem: Dragging is jittery or imprecise

**Symptoms**: The object moves erratically or lags behind the pointer.

**Solution**:
1. Ensure the input system is reading the correct pointer position.
2. Check whether the component is applying smoothing or interpolation.
3. Verify the object is not being simultaneously moved by another script or layout.

---

### Problem: Object exits the allowed area

**Symptoms**: The draggable object can move outside its intended bounds.

**Solution**:
1. Confirm bounds are enabled in the `Draggable` component.
2. Make sure the boundary target is assigned correctly.
3. Adjust the boundary coordinates or container references.

---

### Problem: Drag does not start

**Symptoms**: The object does not respond to pointer down events.

**Solution**:
1. Verify the object has an interactable collider or UI raycast target.
2. Ensure the pointer input module is active and input events are routed.
3. Check for conflicting UI elements blocking the pointer.

## Related Topics

- [Flex Panel](../Panels/flex-panel.md)
- [Swap Panel](../Panels/swap-panel.md)

## Getting Help

- Check [Documentation](../README.md)
- Open an issue if the drag interaction is not working as expected

---

**Last updated**: May 28, 2026  
**Component**: Draggable
