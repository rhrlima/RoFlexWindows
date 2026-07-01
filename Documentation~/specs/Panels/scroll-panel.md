# ScrollPanel / RoScrollPanel — Integration Spec

[Back to specs index](../README.md)

## Purpose

Scrolling container for panel content. `RoScrollPanel` is a styled `ScrollRect` subclass. `ScrollPanel` is a legacy debug stub.

## Current Public Interface

### RoScrollPanel

Inherits full `ScrollRect` API (`content`, `viewport`, `vertical`, `horizontal`, `onValueChanged`, etc.). No RO-specific additions.

### ScrollPanel

| Member | Description |
| --- | --- |
| `OnScrollValueChange()` | Logs content position and floors Y — debug only. |

## Desired Integration Pattern

Use `RoScrollPanel` (or Unity `ScrollRect`) directly:

```csharp
using RO_Flex_UI.Panels;

scrollPanel.content = contentRect;
scrollPanel.onValueChanged.AddListener(host.OnScrollChanged);
```

`ScrollPanel` should be removed, finished, or marked obsolete. `ListPanel` handles its own viewport scrolling for list rows.

[Back to specs index](../README.md)
