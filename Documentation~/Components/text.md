# Text

[Go Back](../README.md)

## Description

The Text prefab is a styled `TextMeshProUGUI` for labels that do not need package
specific runtime behavior. Configure it with the inherited TextMesh Pro inspector
and API.

## Displayed Data

| Data | Source | Description |
| --- | --- | --- |
| Text | `TMP_Text.text` | Displayed string. |
| Font | `TMP_Text.font` | Assigned TextMesh Pro font asset. |
| Color | `Graphic.color` | Base text color. |

## Public API

The prefab adds no API beyond `TextMeshProUGUI`.

## Examples

```csharp
label.text = title;
label.color = Color.white;
```

## Related Components

- [RoText](ro-text.md)

[Go Back](../README.md)
