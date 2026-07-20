# GearPanel

[Go Back](../README.md)

## Description

`GearPanel` creates matching columns of `IconText` equipment slots. During
`Awake`, it clones `slotsPerPanel` entries into each configured panel and
reverses the arrangement of entries on the right side.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| Slot template | `IconText` | Inactive entry cloned for every equipment slot. |
| Left slots | `RectTransform` | Parent for the first set of clones. |
| Right slots | `RectTransform` | Parent for the mirrored set of clones. |

## Public API

### Methods

| Method | Description |
| --- | --- |
| `EnsureReferences()` | Validates the template and both destination panels. |

`GearPanel` has no runtime method for rebuilding or retrieving slots. Configure
its slot count and references before `Awake`, then populate the generated child
`IconText` components from application code.

## Examples

```csharp
foreach (var slot in gearPanel.GetComponentsInChildren<IconText>())
{
    if (slot.gameObject.activeSelf)
        slot.Clear();
}
```

## Related Components

- [IconText](../Components/icon-text.md)

[Go Back](../README.md)
