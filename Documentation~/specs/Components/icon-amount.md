# IconAmount — Integration Spec

[Back to specs index](../README.md)

## Purpose

Displays an icon sprite with an optional numeric amount label. Used as inventory slots, currency displays, drag sources/proxies, and socket contents inside `ItemLine`.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `EnsureReferences()` | `bool` | Validates `iconSprite` and `iconText` references. |
| `Assign(Sprite, int)` | Method | Sets sprite and amount text. Clears when sprite is null or amount ≤ 0. |
| `Clear()` | Method | Nulls sprite and clears text. |
| `SetActive(bool)` | Method | Sets visibility flag and GameObject active state. |
| `ToggleText(bool)` | Method | Shows or hides the amount text GameObject. |
| `Sprite` | `Sprite` | Direct sprite get/set (bypasses presentation policy). |
| `Text` | `string` | Direct text get/set (bypasses presentation policy). |
| `IsVisible` | `bool` | Last value passed to `SetActive`. |

## Desired Integration Pattern

External code should bind slot data through `Assign` or a future unified setter, never by reaching into child `Image` / `TMP_Text` components.

```csharp
using RO_Flex_UI.Components;
using UnityEngine;

public void BindSlot(IconAmount slot, Sprite icon, int amount)
{
    if (icon == null || amount <= 0)
    {
        slot.Clear();
        slot.SetActive(false);
        return;
    }

    slot.SetActive(true);
    slot.Assign(icon, amount);
}
```

Presentation rules consumers should be able to rely on:

- Amount `1` hides the text label (icon-only display).
- `Clear()` deactivates icon and text children and sets `IsVisible` to false.
- Property setters (`Sprite`, `Text`) apply the same rules as `Assign`.

For non-numeric labels (e.g. stack text from `DraggableItem` when `amount <= 0`), a string overload or `SetPresentation(Sprite, string, int?)` should be used instead of setting `Text` directly.

## Related Scenarios

- [Inventory grid](../../integration-review.md#scenario-inventory-grid)
- [Item line with gem sockets](../../integration-review.md#scenario-item-line-sockets)
- [Item tooltip on hover](../../integration-review.md#scenario-item-tooltip)

[Back to specs index](../README.md)
