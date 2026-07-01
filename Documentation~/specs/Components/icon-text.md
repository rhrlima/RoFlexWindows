# IconText — Integration Spec

[Back to specs index](../README.md)

## Purpose

Displays an icon and a text label in a horizontal or vertical layout. Used in gear slots, list rows, and any compact icon+label row.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `EnsureReferences()` | `bool` | Validates icon, text, and layout group references. |
| `FlipElements(bool)` | Method | Reverses layout order (icon after text). |
| `Text` | `string` | Label text. |
| `Sprite` | `Sprite` | Icon sprite. |

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Components;
using UnityEngine;

public void BindGearSlot(IconText slot, Sprite icon, string label, bool iconOnRight = false)
{
    slot.FlipElements(iconOnRight);
    slot.Sprite = icon;
    slot.Text = label;
}
```

For empty slots, consumers should call a future `Clear()` that hides or resets both elements consistently.

```csharp
// Desired — not yet available
slot.Set(icon, label);
slot.Clear();
```

## Related Scenarios

- [Equipment gear display](../../integration-review.md#scenario-equipment-gear)

[Back to specs index](../README.md)
