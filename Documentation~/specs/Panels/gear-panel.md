# GearPanel — Integration Spec

[Back to specs index](../README.md)

## Purpose

Instantiates mirrored gear slot rows on left and right sides from an `IconText` template. Used for character equipment displays.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `EnsureReferences()` | `bool` | Validates template and left/right panel references. |

Slots are created in `Awake` from `slotsPerPanel` (editor-only count). No runtime slot access or binding API.

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Components;
using RO_Flex_UI.Panels;
using UnityEngine;

for (var i = 0; i < gearPanel.SlotCount; i++) // desired
{
    var slot = gearPanel.GetSlot(i);           // desired
    var piece = equipment.GetSlot(i);
    slot.Sprite = piece?.Icon;
    slot.Text = piece?.Name ?? string.Empty;
}
```

Alternatively, `gearPanel.ConfigureGear(int index, Sprite icon, string label)` for one-call binding after initialization.

## Related Scenarios

- [Equipment gear display](../../integration-review.md#scenario-equipment-gear)

[Back to specs index](../README.md)
