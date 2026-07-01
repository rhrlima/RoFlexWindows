# ItemDescriptionPanel — Integration Spec

[Back to specs index](../README.md)

## Purpose

Detail panel showing item title, description, splash image, and an optional preview action. Used when inspecting inventory or shop items.

## Current Public Interface

| Member | Type | Description |
| --- | --- | --- |
| `EnsureReferences()` | `bool` | **Throws `NotImplementedException`.** |

Serialized references exist (`titleText`, `descriptionText`, `splashImage`, `previewButton`) but are not exposed publicly. Namespace is `RO_Flex_UI.Components` while the file lives under `Panels/`.

## Desired Integration Pattern

```csharp
using RO_Flex_UI.Components; // desired: RO_Flex_UI.Panels

panel.Title = item.Name;
panel.Description = item.Description;
panel.Image = item.Splash;
panel.OnPreview.AddListener(() => host.PreviewItem(item));
```

`EnsureReferences()` must validate references and return `false` on failure — never throw.

## Related Scenarios

- [Item detail view](../../integration-review.md#scenario-item-detail)

[Back to specs index](../README.md)
