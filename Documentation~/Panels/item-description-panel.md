# ItemDescriptionPanel

[Go Back](../README.md)

## Description

`ItemDescriptionPanel` groups a title, description, splash image, and preview
button for an item-details layout. The current class exposes no data-assignment
API, and its `EnsureReferences()` method throws `NotImplementedException`.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| Title | `TMP_Text` | Inspector-assigned title label. |
| Description | `TMP_Text` | Inspector-assigned description label. |
| Splash | `Image` | Inspector-assigned item image. |
| Preview action | `RoButton` | Inspector-assigned preview button. |

## Public API

### Methods

| Method | Description |
| --- | --- |
| `EnsureReferences()` | Currently throws `NotImplementedException`; do not call it at runtime. |

## Examples

The component does not currently expose its child references. Configure the
provided prefab in the Inspector, or bind child `TMP_Text`, `Image`, and
`RoButton` components from application-specific code.

## Related Components

- [RoButton](../Components/ro-button.md)

[Go Back](../README.md)
