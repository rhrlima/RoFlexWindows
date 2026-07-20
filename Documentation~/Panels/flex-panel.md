# FlexPanel

[Go Back](../README.md)

## Description

`FlexPanel` configures `LayoutElement` values on its child entries. Each entry
can reserve a fixed size or receive a proportional share of flexible space
along the selected vertical or horizontal axis.

## Displayed Data

`FlexPanel` displays no data. A parent `VerticalLayoutGroup` or
`HorizontalLayoutGroup` consumes the values it writes to child layout elements.

## Configuration

| Field | Description |
| --- | --- |
| `orientation` | Chooses whether height or width layout values are applied. |
| `entries` | Ordered child configurations. Empty lists are populated from direct children. |
| Entry `mode` | Uses either an exact fixed size or a flexible proportion. |
| Entry `fixedSize` | Minimum and preferred size used by fixed entries. |
| Entry `proportion` | Non-negative flexible width or height weight. |

Missing `LayoutElement` components are added automatically. Existing layout
values on each entry are reset before the selected axis is configured.

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `LayoutOrientation` | `Orientation` | Gets or changes the layout axis and reapplies the entries. |

## Examples

```csharp
flexPanel.LayoutOrientation = FlexPanel.Orientation.Horizontal;
```

Configure fixed and proportional entries in the Inspector. Runtime entry
editing is not exposed by the current public API.

[Go Back](../README.md)
