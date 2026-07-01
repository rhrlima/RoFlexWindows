# RoButton — Integration Spec

[Back to specs index](../README.md)

## Purpose

Package-styled `UnityEngine.UI.Button` with required `Image`. Base type for `TabButton` and the selectable surface on `ListItem`.

## Current Public Interface

Inherits the full `Button` / `Selectable` API. Adds:

| Member | Type | Description |
| --- | --- | --- |
| `EnsureReferences()` | `bool` | No-op; always returns true. |

No RO-specific label or icon helpers.

## Desired Integration Pattern

Subscribe to `onClick` and set `interactable` from host code. When prefabs include a child `TMP_Text`, a `Label` property should avoid child lookups:

```csharp
using RO_Flex_UI.Components;

confirmButton.Label = "Confirm";   // desired API
confirmButton.interactable = canConfirm;
confirmButton.onClick.AddListener(OnConfirm);
```

See also [RoButton reference doc](../../Components/ro-button.md) for usage examples.

[Back to specs index](../README.md)
