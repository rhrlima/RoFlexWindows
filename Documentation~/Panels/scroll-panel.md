# Scroll Panel

[Go Back](../README.md)

## Description

The Scroll Panel prefab is a styled Unity `ScrollRect` with horizontal and
vertical scrollbars. It uses Unity's standard scrolling API and does not attach
the package's legacy `ScrollPanel` or `RoScrollPanel` scripts.

## Displayed Data

| Data | Source | Description |
| --- | --- | --- |
| Content | `ScrollRect.content` | RectTransform moved within the viewport. |
| Position | `normalizedPosition` | Current horizontal and vertical scroll position. |

## Public API

The prefab uses the inherited `UnityEngine.UI.ScrollRect` API.

| Member | Type | Description |
| --- | --- | --- |
| `content` | `RectTransform` | Scrollable content container. |
| `viewport` | `RectTransform` | Visible masked area. |
| `horizontalNormalizedPosition` | `float` | Horizontal position from 0 to 1. |
| `verticalNormalizedPosition` | `float` | Vertical position from 0 to 1. |
| `onValueChanged` | `ScrollRectEvent` | Invoked when the normalized position changes. |

## Examples

```csharp
scrollRect.verticalNormalizedPosition = 1f;
scrollRect.onValueChanged.AddListener(SaveScrollPosition);
```

## Related Panels

- [ListPanel](list-panel.md)

[Go Back](../README.md)
