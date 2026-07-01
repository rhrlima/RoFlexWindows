# Integration Specs

Per-component and per-panel integration specifications: purpose, public interface, and the integration pattern external code should follow.

When creating or changing a spec, follow [UI Element Development Guidelines](../ui-element-guidelines.md).

Gaps between these specs and the current implementation are tracked as [open points](../integration-review.md#open-points-from-specs) in the integration review.

## Components

| Spec | Type |
| --- | --- |
| [IconAmount](Components/icon-amount.md) | Display |
| [IconText](Components/icon-text.md) | Display |
| [ListItem](Components/list-item.md) | List row |
| [ItemLine](Components/item-line.md) | Display |
| [SkillEntry](Components/skill-entry.md) | Display / input |
| [Header](Components/header.md) | Window chrome |
| [RoButton](Components/ro-button.md) | Input |
| [RoToggle](Components/ro-toggle.md) | Input |
| [RoInput](Components/ro-input.md) | Input |
| [RoDropdown](Components/ro-dropdown.md) | Input |
| [RoSlider](Components/ro-slider.md) | Input |
| [ToggleSwitch](Components/toggle-switch.md) | Input |
| [Draggable / DraggableBase](Components/draggable.md) | Interaction |
| [DraggableItem](Components/draggable-item.md) | Interaction |
| [DropZone](Components/drop-zone.md) | Interaction |
| [Tooltip](Components/tooltip.md) | Display |
| [TooltipPanel](Components/tooltip-panel.md) | Interaction |
| [Resizable](Components/resizable.md) | Interaction |

## Panels

| Spec | Type |
| --- | --- |
| [ListPanel](Panels/list-panel.md) | Collection |
| [FillPanel](Panels/fill-panel.md) | Grid |
| [GearPanel](Panels/gear-panel.md) | Grid |
| [SkillMinPanel](Panels/skill-min-panel.md) | Collection |
| [ItemDescriptionPanel](Panels/item-description-panel.md) | Detail view |
| [SwapPanel](Panels/swap-panel.md) | Layout |
| [TabsPanel](Panels/tabs-panel.md) | Navigation |
| [TabButton](Panels/tab-button.md) | Navigation |
| [FlexPanel](Panels/flex-panel.md) | Layout |
| [Window](Panels/window.md) | Container |
| [ScrollPanel / RoScrollPanel](Panels/scroll-panel.md) | Scrolling |

## Multi-element scenarios

| Scenario | Specs involved |
| --- | --- |
| [Selectable list](../integration-review.md#scenario-selectable-list) | `ListPanel`, `ListItem` |
| [Inventory grid with drag-and-drop](../integration-review.md#scenario-inventory-grid) | `FillPanel`, `IconAmount`, `DraggableItem`, `DropZone` |
| [Equipment gear display](../integration-review.md#scenario-equipment-gear) | `GearPanel`, `IconText`, `ListPanel` |
| [Skill list with level controls](../integration-review.md#scenario-skill-list) | `SkillMinPanel`, `SkillEntry` |
| [Item tooltip on hover](../integration-review.md#scenario-item-tooltip) | `TooltipPanel`, `Tooltip`, `IconAmount` |
| [Tabbed panel host](../integration-review.md#scenario-tabbed-panel) | `TabsPanel`, `TabButton` |
| [Movable / resizable window](../integration-review.md#scenario-window) | `Window`, `Header`, `Draggable`, `Resizable` |
| [Item line with gem sockets](../integration-review.md#scenario-item-line-sockets) | `ItemLine`, `IconAmount` |
| [Item detail view](../integration-review.md#scenario-item-detail) | `ItemDescriptionPanel` |
| [Panel group swapping](../integration-review.md#scenario-panel-swap) | `SwapPanel` |

[Back to documentation index](../README.md)
