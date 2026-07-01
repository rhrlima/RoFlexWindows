# RO Flex UI — Components & Panels Integration Review

Review of `RO Flex UI/Runtime/Scripts/Components` and `RO Flex UI/Runtime/Scripts/Panels`, focused on **data-display APIs**, **external integration**, and **coupling**.

**Severity legend:** **High** (blocks or breaks integration), **Medium** (friction or inconsistency), **Low** (polish / maintainability).

Reviewed: 2026-07-01.

Per-component integration specs (purpose, public interface, desired patterns): [specs/](specs/README.md).

---

## Cross-cutting findings

| Issue | Severity | Affected types |
|---|---|---|
| `IComponent` / `IPanel` only expose `EnsureReferences()` — no shared data-binding or event contract | Medium | Most types |
| Inconsistent namespaces (`global` vs `RO_Flex_UI.*`) | Low | `Tooltip`, `TooltipPanel`, `FillPanel2` |
| File/class name mismatches | Medium | `FillPanel.cs` → `FillPanel2` |
| Consumers often reach into children (`GetComponentInChildren<TMP_Text>`) instead of component APIs | Medium | `ListItem`, `GearPanel`, `FillPanel2` |
| `IPanel.EnsureReferences()` throws `NotImplementedException` on some panels | High | `ItemDescriptionPanel`, `SwapPanel` |
| Thin wrappers (`RoInput`, `RoToggle`, `RoDropdown`, `RoScrollPanel`) inherit Unity/TMP APIs with no RO-specific surface | Low | Wrappers |

---

## Components

### IconAmount

| Severity | Finding | Suggestion |
|---|---|---|
| **High** | `Assign` / `Clear` / setters do not match the tested presentation contract (visibility, `ToggleText` for amount `1`, deactivating children on clear). Tests in `DropZoneTests.IconAmountAssignsAndClearsPresentation` expect this behavior. | Centralize presentation in `RefreshPresentation()`; call from `Assign`, `Clear`, `SetActive`, and route `Sprite`/`Text` setters through it. |
| **Medium** | No `Assign(Sprite, string)` overload — `DraggableItem` bypasses `Assign` when `amount <= 0`. | Add string overload or a single `SetPresentation(Sprite, string, int?)` API. |
| **Medium** | `Sprite` / `Text` setters skip `EnsureReferences()` and visibility policy. | Validate and refresh like other public methods. |
| **Low** | `ToggleText` does not guard references. | Call `EnsureReferences()` first. |
| **Low** | Unused `using System.Globalization`. | Remove. |

---

### IconText

| Severity | Finding | Suggestion |
|---|---|---|
| **Medium** | Only `Text` and `Sprite` — no combined setter, no visibility/clear API. | Add `Set(Sprite, string)` and `Clear()` for parity with `IconAmount`. |
| **Low** | Property setters skip `EnsureReferences()`. | Guard or document as editor-only. |

---

### ListItem

| Severity | Finding | Suggestion |
|---|---|---|
| **High** | No public API to set displayed data — `ListExample` uses `GetComponentInChildren<TMPro.TextMeshProUGUI>()`. | Add virtual `Bind(object data)` or typed properties (`Label`, `Icon`, etc.) on `ListItem` / subclasses. |
| **Medium** | Tight coupling to `ListPanel` via `BindToPanel`, `parentPanel?.Notify*`. Hard to reuse outside a list. | Introduce `IListItemHost` with focus/activate callbacks; keep `ListPanel` as default host. |
| **Medium** | `Awake` calls `OnEnable()` manually; Unity also calls `OnEnable` → duplicate `onClick` listeners, double focus on click. | Use `Awake` for `EnsureReferences()` only; register listeners in `OnEnable`. |
| **Low** | No `Index`, `Data`, or `IsSelected` for external code. | Expose index (from panel) and optional payload reference. |

---

### ItemLine

| Severity | Finding | Suggestion |
|---|---|---|
| **High** | Only `Text` is public — `sockets` are serialized with no runtime API (`AssignSocket`, `SetSocketCount`, `ClearSockets`). | Add methods to populate sockets from data; expose read-only socket access. |
| **Medium** | `Socket` type is public but list is private — external code cannot configure without reflection. | Add `ConfigureSockets(IReadOnlyList<SocketData>)` or similar. |
| **Low** | `numSockets` is read-only count with no way to change socket count at runtime. | Add `ResizeSockets(int count)`. |

---

### SkillEntry

| Severity | Finding | Suggestion |
|---|---|---|
| **Medium** | `IsFixedLevel` is stored but never disables buttons or blocks handlers. | Disable `skillLevelUp`/`skillLevelDown` when fixed; optionally hide cost controls for passive. |
| **Medium** | `onSkillLevelUp` is declared but never invoked — dead API. | Wire into level-up flow or remove. |
| **Medium** | Uses raw `Button` instead of `RoButton` — inconsistent with package conventions. | Switch to `RoButton` or document why not. |
| **Low** | No batch `Set(string name, string level, string cost, bool passive, bool fixed)`. | Add `Configure(SkillViewModel)` for one-call binding. |

---

### Header

| Severity | Finding | Suggestion |
|---|---|---|
| **Medium** | Only `Text` is bindable — no API to show/hide or enable individual header buttons. | Add `SetButtonVisible(HeaderButton, bool)` / `SetButtonInteractable(...)`. |
| **Low** | Serialized events use `Button.ButtonClickedEvent` while buttons are `RoButton`. | Use `RoButton`-compatible event types for consistency. |

---

### RoButton

| Severity | Finding | Suggestion |
|---|---|---|
| **Low** | `EnsureReferences()` is a no-op — fine for a thin wrapper. | Optionally validate `targetGraphic`. |
| **Low** | No `Text`/`Label` helper — consumers still hunt child TMP. | Add optional `TMP_Text` reference + `Label` property if prefabs are consistent. |

---

### RoToggle / RoInput / RoDropdown

| Severity | Finding | Suggestion |
|---|---|---|
| **Low** | Empty subclasses — integration is entirely via Unity/TMP APIs. | Acceptable if documented; add `IComponent` only if reference validation is needed. |

---

### RoSlider

| Severity | Finding | Suggestion |
|---|---|---|
| **Medium** | Pointer handlers can run when `dragArea` is null if `EnsureReferences` failed in `Awake`. | Disable component or guard all pointer paths after failed validation. |
| **Low** | Custom events (`onDecreaseClick`, etc.) duplicate `onValueChanged` semantics. | Document when to use each; consider unifying. |

---

### ToggleSwitch

| Severity | Finding | Suggestion |
|---|---|---|
| **Medium** | `OnMove` is empty — blocks inherited keyboard/gamepad slider navigation with no replacement. | Handle submit explicitly or delegate to toggle logic. |
| **Low** | `IsOn` reflects animated slider value, not committed logical state during animation. | Expose `CommittedState` / use `targetState` for logical reads. |
| **Low** | Does not implement `IComponent`. | Add if reference validation becomes necessary. |

---

### DraggableItem / DropZone / DraggableBase

| Severity | Finding | Suggestion |
|---|---|---|
| **Medium** | `DraggableItem.OnDisable` finishes drag with `notify: false` — no `onDropRejected` / `onEndDrag` when disabled mid-drag. | Add cancellation path that notifies listeners, or document and expose `CancelDrag()`. |
| **Medium** | `DropZone` events pass `DraggableItem`, not `DragPayload` — consumers must read `CurrentPayload` or re-query. | Add `UnityEvent<DragPayload>` or include payload in event args. |
| **Medium** | `DropZone.CanDrop` / `Drop` default to `true` — easy to forget override. | Default to `false` or require subclass; document base behavior. |
| **Low** | `DragPayload.Data` / `Source` are `object` — flexible but error-prone. | Offer generic helpers or typed wrapper interfaces. |
| **Low** | `DraggableBase` FIXME about `SetAsLastSibling` — window z-order partially handled in `Window.OnPointerDown`. | Centralize in a window manager when available. |

---

### Tooltip / TooltipPanel

| Severity | Finding | Suggestion |
|---|---|---|
| **Medium** | `Tooltip.Update` runs every frame with no null guard if `EnsureReferences` failed. | Disable component on failed validation; guard `Update`. |
| **Medium** | `SetText` / `ShowTooltip` / `RefreshSize` skip validation. | Guard public methods. |
| **Medium** | `TooltipPanel` has no API to set tooltip text — text must be set on `Tooltip` directly. | Add `SetTooltipText(string)` on `TooltipPanel` or bind to `IconAmount` content. |
| **Low** | Global namespace (not `RO_Flex_UI.Components`). | Move into package namespace. |
| **Low** | `Tooltip` follows mouse via `Input.mousePosition` — poor for canvas-scaled UI / multi-display. | Convert through canvas / `RectTransformUtility`. |

---

### Resizable

| Severity | Finding | Suggestion |
|---|---|---|
| **Medium** | `OnBeginDrag` does not filter left button; `OnDrag` does — asymmetric resize sessions. | Apply same button gate at drag start. |
| **Medium** | Public `StepSize` setter bypasses `[Min(1f)]`; division by `stepSize` in `OnDrag`. | Clamp in setter and `OnValidate`. |
| **Low** | Unnecessary `using RO_Flex_UI.Panels`. | Remove unused import. |
| **Low** | `borderOffset` marked FIXME — snap math may be wrong. | Compute from layout/padding or document manual setup. |

---

## Panels

### ListPanel

| Severity | Finding | Suggestion |
|---|---|---|
| **Medium** | No `Count`, `GetItem(int)`, `RemoveItem`, `IndexOf`, or panel-level focus/activate events. | Add read API + `UnityEvent<ListItem>` for focus/activate. |
| **Medium** | `NotifyItemActivated` logs `Debug.Log` — noisy in production integrations. | Remove or gate behind `#if UNITY_EDITOR` / debug flag. |
| **Medium** | `Clear()` destroys GameObjects — surprising if caller still holds references. | Document ownership; consider `Clear(destroyItems: bool)`. |
| **Low** | Docs mention `initialItems` / `Awake` registration; code uses `items` in `Start`. | Align docs with `items` + `Start` behavior. |
| **Low** | FIXME/TODO: scroll drag conflicts, list flicker. | Track as known UX issues. |

---

### FillPanel2 (`FillPanel.cs`)

| Severity | Finding | Suggestion |
|---|---|---|
| **High** | Class is `FillPanel2` in global namespace; tests resolve `"FillPanel, Assembly-CSharp"` — naming/assembly fragility. | Rename to `FillPanel`, move to `RO_Flex_UI.Panels`. |
| **High** | No API to access or bind cell content — only `SetFilledCells` + `Refresh` toggles active count. | Expose `IReadOnlyList<TCell>`, `GetCell(int)`, `ForEachCell(Action<int, GameObject>)`, or typed cell component. |
| **Medium** | `cellTemplate` is raw `GameObject` — consumers must know internal structure. | Use typed template (`IconAmount`, `DraggableItem`, etc.). |
| **Medium** | Grid recalculates every frame when viewport size changes (`Update` polling). | Prefer `OnRectTransformDimensionsChange` only, or throttle. |
| **Low** | Implements `IPanel` but is usable; `EnsureReferences` is solid. | Keep; extend binding API. |

---

### GearPanel

| Severity | Finding | Suggestion |
|---|---|---|
| **High** | Instantiates slots in `Awake` but exposes no way to get or update them. | Expose `IReadOnlyList<IconText> Slots` or `GetSlot(int index)`. |
| **Medium** | No `ConfigureGear(int index, Sprite, string)` — external code cannot display gear data. | Add slot binding API after `InitializePanels`. |
| **Low** | `slotsPerPanel` is editor-only — cannot resize at runtime. | Add `Initialize(int slotsPerSide)` if needed. |

---

### SkillMinPanel

| Severity | Finding | Suggestion |
|---|---|---|
| **High** | Stub: only public fields `skillEntryPrefab` and `contentPanel` — no population, binding, or events. | Implement `Clear`, `AddSkills`, `AddSkills<T>(IEnumerable<T>, Action<SkillEntry,T>)` mirroring `ListPanel`. |
| **Medium** | Does not implement `IPanel`. | Add `IPanel` + `EnsureReferences`. |

---

### ItemDescriptionPanel

| Severity | Finding | Suggestion |
|---|---|---|
| **High** | `EnsureReferences()` throws `NotImplementedException` — breaks `IPanel` contract. | Implement validation; never throw from interface methods. |
| **High** | No public properties for title, description, image, or preview button events. | Add `Title`, `Description`, `Image`, `OnPreview` (or `RoButton.onClick` accessor). |
| **Medium** | File under `Panels/`, namespace `RO_Flex_UI.Components`, unused `using RO_Flex_UI.Panels`. | Align folder, namespace, and usings. |

---

### SwapPanel

| Severity | Finding | Suggestion |
|---|---|---|
| **High** | `EnsureReferences()` throws `NotImplementedException`. | Implement or remove `IPanel` until ready. |
| **Medium** | `onSwapEvent` is private serialized — no C# accessor for external subscription. | Expose `UnityEvent OnSwapped { get; }`. |
| **Medium** | `activeGroup` is public field — breaks encapsulation. | Use property with `SwapByGroup` as sole mutator. |
| **Low** | `GetNextGroup` / `GetPreviousGroup` naming sounds like getters. | Rename to `SwapToNextGroup` / `SwapToPreviousGroup`. |
| **Low** | Docs for swap panel are minimal. | Document group/index API and events. |

---

### TabsPanel

| Severity | Finding | Suggestion |
|---|---|---|
| **Medium** | No API to set tab labels, add/remove tabs, or query tab count at runtime. | Add `SetTabLabel(int, string)`, `TabCount`, dynamic registration if needed. |
| **Medium** | Per-entry `onPanelEnter` / `onPanelExit` are inspector-only — no panel-level unified event. | Add `TabEvent OnTabChanged` at panel level. |
| **Low** | Does not implement `IPanel`. | Optional unless reference validation is required. |
| **Low** | Tab button listeners registered in `Start` only — runtime-added entries won't wire. | Add `RegisterTab` for dynamic tabs. |

---

### TabButton

| Severity | Finding | Suggestion |
|---|---|---|
| **Medium** | No `Label` / `Text` API for tab captions. | Add `TMP_Text` reference + `Label` property. |
| **Low** | `SetActive` controls sprite state, not `GameObject` active — name collides with `GameObject.SetActive`. | Rename to `SetSelected(bool)` (coordinate with `TabsPanel`). |

---

### FlexPanel

| Severity | Finding | Suggestion |
|---|---|---|
| **Medium** | Layout is editor-driven; `entries` are private — no runtime API to change flex/fixed sizes. | Expose `SetEntryProportion(int, float)`, `SetOrientation`, `Rebuild()`. |
| **Low** | Auto-adds `LayoutElement` at runtime — can fight manually tuned layouts. | Document side effects. |

---

### Window

| Severity | Finding | Suggestion |
|---|---|---|
| **Medium** | Window flags (`isDraggable`, `isResizable`, `resetToCenter`, etc.) are serialize-only — no runtime API. | Add properties/methods for host code (e.g. modal vs movable). |
| **Medium** | `LateUpdate` clamping every frame when `keepWindowInScreen` — cost for many windows. | Clamp on drag end / resize end only, or event-driven. |
| **Low** | TODO for ESC / window manager — integration gap for modal stacks. | Implement `IWindow` manager when ready. |

---

### ScrollPanel / RoScrollPanel

| Severity | Finding | Suggestion |
|---|---|---|
| **High** | `ScrollPanel` is a debug stub (`Debug.Log` in `OnScrollValueChange`) — not production-ready. | Remove, finish, or mark obsolete. |
| **Low** | `RoScrollPanel` is empty `ScrollRect` subclass. | Document as styling anchor or add RO-specific scroll behavior. |

---

## Suggested fix priority

1. **Immediate** — Implement or remove `NotImplementedException` in `ItemDescriptionPanel` and `SwapPanel`; flesh out `ItemDescriptionPanel` and `SkillMinPanel` public APIs.
2. **Short term** — Add data-binding surfaces: `ListItem.Bind`, `GearPanel` slot access, `FillPanel2` cell access, `ItemLine` socket API.
3. **Stabilization** — Fix `ListItem` double listener, `IconAmount` presentation consistency, `TabsPanel`/`TabButton` labeling.
4. **Cleanup** — Namespace alignment (`Tooltip`, `FillPanel2` → `FillPanel`), remove dead `ScrollPanel`, trim unused imports.

---

## Integration pattern to aim for

External code should ideally follow one pattern per component:

```csharp
// Display
panel.Clear();
panel.AddItems(data, (row, item) => row.Bind(item));

// Interaction
row.OnItemActivated.AddListener(() => host.OnRowChosen(row));
slider.onValueChanged.AddListener(host.OnVolumeChanged);
```

Today, many panels stop at layout/structure and leave binding to `GetComponentInChildren`, reflection (`ListPanelTests`), or inspector wiring — that is the main integration gap this review surfaces.

---

## Open points from specs

Gaps between the [integration specs](specs/README.md) and the current implementation. Each item links to the spec that defines the desired pattern.

| Spec | Severity | Open point |
| --- | --- | --- |
| [IconAmount](specs/Components/icon-amount.md) | **High** | `Assign` / `Clear` / setters do not enforce the presentation contract (hide text for amount `1`, deactivate children on clear, `IsVisible` after clear). Tests in `DropZoneTests.IconAmountAssignsAndClearsPresentation` already encode the contract. |
| [IconAmount](specs/Components/icon-amount.md) | Medium | No `Assign(Sprite, string)` or unified `SetPresentation` — `DraggableItem` bypasses `Assign` when `amount <= 0`. |
| [IconText](specs/Components/icon-text.md) | Medium | No `Set(Sprite, string)` or `Clear()` for parity with `IconAmount`. |
| [ListItem](specs/Components/list-item.md) | **High** | No `Bind` API — consumers use `GetComponentInChildren<TMP_Text>` (see `ListExample`). |
| [ListItem](specs/Components/list-item.md) | Medium | Hard-coded `ListPanel` coupling; needs `IListItemHost` for reuse outside lists. |
| [ListItem](specs/Components/list-item.md) | Medium | `Awake` calls `OnEnable()` manually → duplicate `onClick` listeners and double focus. |
| [ItemLine](specs/Components/item-line.md) | **High** | No socket population API (`ConfigureSockets`, `AssignSocket`, `ResizeSockets`). |
| [SkillEntry](specs/Components/skill-entry.md) | Medium | `IsFixedLevel` not enforced; `onSkillLevelUp` never invoked; no `Configure(...)` batch setter. |
| [Header](specs/Components/header.md) | Medium | No per-button visibility or interactable API. |
| [RoButton](specs/Components/ro-button.md) | Low | No `Label` helper for child `TMP_Text`. |
| [RoSlider](specs/Components/ro-slider.md) | Medium | Pointer handlers can run when `dragArea` is null after failed validation. |
| [ToggleSwitch](specs/Components/toggle-switch.md) | Medium | Empty `OnMove` blocks gamepad navigation; no `CommittedState` for logical reads during animation. |
| [DraggableItem](specs/Components/draggable-item.md) | Medium | `OnDisable` finishes drag with `notify: false` — no cancellation event. |
| [DropZone](specs/Components/drop-zone.md) | Medium | Events pass `DraggableItem`, not `DragPayload`; default `CanDrop`/`Drop` are permissive (`true`). |
| [Tooltip](specs/Components/tooltip.md) | Medium | `Update` and public methods lack guards after failed `EnsureReferences`; mouse positioning ignores canvas scale. |
| [TooltipPanel](specs/Components/tooltip-panel.md) | Medium | No `SetTooltipText` — text must be set on `Tooltip` directly. |
| [Resizable](specs/Components/resizable.md) | Medium | `OnBeginDrag` does not filter left button; `StepSize` setter bypasses `[Min(1f)]`. |
| [ListPanel](specs/Panels/list-panel.md) | Medium | No `Count`, `GetItem`, panel-level focus/activate events; `Clear()` always destroys items. |
| [FillPanel](specs/Panels/fill-panel.md) | **High** | No cell binding API; `FillPanel2` naming/namespace fragility. |
| [GearPanel](specs/Panels/gear-panel.md) | **High** | No slot access or `ConfigureGear` after `Awake` instantiation. |
| [SkillMinPanel](specs/Panels/skill-min-panel.md) | **High** | Stub — no `Clear`, `AddSkills`, events, or `IPanel`. |
| [ItemDescriptionPanel](specs/Panels/item-description-panel.md) | **High** | `EnsureReferences()` throws; no public `Title`/`Description`/`Image`/`OnPreview`. |
| [SwapPanel](specs/Panels/swap-panel.md) | **High** | `EnsureReferences()` throws; `onSwapEvent` not exposed; `activeGroup` is a public field. |
| [TabsPanel](specs/Panels/tabs-panel.md) | Medium | No runtime tab labels, dynamic `RegisterTab`, or panel-level `OnTabChanged`. |
| [TabButton](specs/Panels/tab-button.md) | Medium | No `Label` API; `SetActive` name collides with `GameObject.SetActive`. |
| [FlexPanel](specs/Panels/flex-panel.md) | Medium | No runtime `SetEntryProportion` / `Rebuild` API. |
| [Window](specs/Panels/window.md) | Medium | Window flags are serialize-only; no window manager / ESC handling; per-frame clamping cost. |
| [ScrollPanel](specs/Panels/scroll-panel.md) | **High** | `ScrollPanel` is a debug stub — not production-ready. |

---

## Multi-element scenarios

Requirements for flows that combine multiple components or panels. Each scenario lists what the host must do today versus the desired integration path.

### Scenario: Selectable list

**Elements:** `ListPanel`, `ListItem` (subclass or configured prefab), host controller.

**Requirements:**

- Host clears the panel, binds data per row, and reacts to row activation.
- Rows must expose label/icon (or arbitrary content) without child component lookups.
- Panel should expose item count, index lookup, and unified focus/activate events.

**Today:** `AddItems<T>(data, bind)` works, but `bind` uses `GetComponentInChildren` because `ListItem` has no `Bind`. `ListItem.Awake` may register duplicate click listeners.

**Desired:**

```csharp
listPanel.Clear();
listPanel.AddItems(entries, (row, entry) => row.Bind(entry));
listPanel.OnItemActivated.AddListener(host.OnRowChosen);
```

Spec: [ListPanel](specs/Panels/list-panel.md), [ListItem](specs/Components/list-item.md).

---

### Scenario: Inventory grid

**Elements:** `FillPanel`, `IconAmount` (per cell), optional `DraggableItem` + `DropZone` per cell, host inventory service.

**Requirements:**

- Grid capacity follows viewport size; filled cell count drives overflow expansion.
- Each visible cell shows icon + amount from inventory data.
- Optional drag-and-drop: configure `DraggableItem` payload, validate drops per zone, refresh grid on accept.

**Today:** `SetFilledCells` + `Refresh` manage layout only. Cell content requires internal `GameObject` list access or prefab structure knowledge. `FillPanel2` class/assembly naming is fragile.

**Desired:**

```csharp
fillPanel.SetFilledCells(inventory.Count);
fillPanel.Refresh();
fillPanel.ForEachCell((i, cell) => cell.Bind(inventory[i]));
```

Spec: [FillPanel](specs/Panels/fill-panel.md), [IconAmount](specs/Components/icon-amount.md), [DraggableItem](specs/Components/draggable-item.md), [DropZone](specs/Components/drop-zone.md).

---

### Scenario: Equipment gear

**Elements:** `GearPanel` or `ListPanel` with `IconText` rows, host equipment service.

**Requirements:**

- Fixed slot count per side (or configurable at runtime).
- Each slot shows equipped item icon and name; empty slots show blank or placeholder.
- Optional: slot click opens item detail or triggers unequip.

**Today:** `GearPanel` instantiates slots in `Awake` with no getter API. `ListExample` uses `ListPanel` + `IconText` as a workaround.

**Desired:**

```csharp
gearPanel.ConfigureGear(slotIndex, piece.Icon, piece.Name);
```

Spec: [GearPanel](specs/Panels/gear-panel.md), [IconText](specs/Components/icon-text.md).

---

### Scenario: Skill list

**Elements:** `SkillMinPanel`, `SkillEntry` per row, host skill service.

**Requirements:**

- Populate scrollable list from skill collection.
- Per row: name, level, cost, passive/fixed flags; wire level-up/down to host logic.
- Fixed-level and passive skills disable appropriate controls.

**Today:** `SkillMinPanel` exposes only prefab and content transform fields. Population is manual.

**Desired:**

```csharp
skillMinPanel.Clear();
skillMinPanel.AddSkills(skills, (entry, skill) => entry.Configure(skill));
```

Spec: [SkillMinPanel](specs/Panels/skill-min-panel.md), [SkillEntry](specs/Components/skill-entry.md).

---

### Scenario: Item tooltip

**Elements:** `TooltipPanel` on hover target, shared `Tooltip` instance, optional sibling `IconAmount`.

**Requirements:**

- Show tooltip on pointer enter when target has content and is visible.
- Hide during drag and when `IconAmount.IsVisible` is false.
- Host sets description text without reaching past `TooltipPanel`.

**Today:** Text is set on `Tooltip` globally before hover. `TooltipPanel` has no text API.

**Desired:**

```csharp
tooltipPanel.SetTooltipText(host.BuildDescription(item));
```

Spec: [TooltipPanel](specs/Components/tooltip-panel.md), [Tooltip](specs/Components/tooltip.md).

---

### Scenario: Tabbed panel

**Elements:** `TabsPanel`, `TabButton` per tab, content `GameObject` per tab, host controller.

**Requirements:**

- Select tab by index or label from code.
- React to tab changes at panel level (not only per-entry Inspector events).
- Support runtime-added tabs when content is dynamic.

**Today:** `SetActiveTab(int)` works. Labels and panel-level events require Inspector wiring or child lookups.

**Desired:**

```csharp
tabsPanel.SetTabLabel(0, "Inventory");
tabsPanel.OnTabChanged.AddListener(host.OnTabChanged);
```

Spec: [TabsPanel](specs/Panels/tabs-panel.md), [TabButton](specs/Panels/tab-button.md).

---

### Scenario: Window

**Elements:** `Window`, `Header`, optional `Draggable` + `Resizable`, host window manager.

**Requirements:**

- Show/hide/center window; optional drag and resize.
- Title and close/minimize wired through `Header`.
- Keep window on screen; bring focused window forward.
- Modal stack: ESC to close, z-order management.

**Today:** `IWindow` show/hide works. Flags are editor-only. Z-order is per-window `SetAsLastSibling`. No manager.

**Desired:**

```csharp
windowManager.Push(modalWindow);
header.OnCloseButtonClick.AddListener(() => windowManager.Pop(modalWindow));
```

Spec: [Window](specs/Panels/window.md), [Header](specs/Components/header.md), [Draggable](specs/Components/draggable.md), [Resizable](specs/Components/resizable.md).

---

### Scenario: Item line sockets

**Elements:** `ItemLine`, `IconAmount` per socket, host item/socket data.

**Requirements:**

- Display item name and N socket slots.
- Populate each socket with gem icon/amount or empty/open state.
- Optionally resize socket count at runtime.

**Today:** Only `Text` is bindable; socket list is private.

**Desired:**

```csharp
itemLine.Text = item.Name;
itemLine.ConfigureSockets(item.Sockets);
```

Spec: [ItemLine](specs/Components/item-line.md), [IconAmount](specs/Components/icon-amount.md).

---

### Scenario: Item detail

**Elements:** `ItemDescriptionPanel`, host selection source (list/grid).

**Requirements:**

- When user selects an item, show title, description, image, and optional preview action.
- Panel validates references without throwing.

**Today:** Panel is a stub; `EnsureReferences()` throws `NotImplementedException`.

**Desired:**

```csharp
detailPanel.Title = item.Name;
detailPanel.Description = item.Description;
detailPanel.Image = item.Splash;
```

Spec: [ItemDescriptionPanel](specs/Panels/item-description-panel.md).

---

### Scenario: Panel swap

**Elements:** `SwapPanel`, multiple child panel groups, host view-mode controller.

**Requirements:**

- Switch visible group by id or cycle next/previous.
- Subscribe to swap events from code.
- Active group readable without mutating public fields.

**Today:** `SwapByGroup` / `SwapByIndex` work. `onSwapEvent` is Inspector-only; `activeGroup` is public.

**Desired:**

```csharp
swapPanel.SwapByGroup(ViewMode.Detailed);
swapPanel.OnSwapped.AddListener(host.OnViewModeChanged);
```

Spec: [SwapPanel](specs/Panels/swap-panel.md).

---

## Related documentation

- [UI Element Development Guidelines](ui-element-guidelines.md) — patterns, validation, public API, integration, and review checklist for new and existing elements.
- [Integration specs](specs/README.md) — per-component purpose, public interface, and desired integration patterns.
- [Component code review (2026-06-25)](Components/component-code-review.md) — earlier per-component tracking doc; several items listed there are already fixed (e.g. `Header.EnsureReferences`, `ItemLine` socket validation, `SkillEntry` reference checks). This report reflects the current source as of 2026-07-01.
