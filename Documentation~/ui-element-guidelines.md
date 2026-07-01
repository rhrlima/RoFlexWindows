# RO Flex UI — UI Element Development Guidelines

Patterns and practices for **new and existing** components and panels. Use this document during design, implementation, review, and documentation — alongside the [feature development workflow](../.cursor/skills/feature-development/SKILL.md).

**Related references:**

- [Integration specs](specs/README.md) — per-type purpose, public interface, desired integration pattern
- [Integration review](integration-review.md) — current gaps, multi-element scenarios, fix priorities
- [Component code review](Components/component-code-review.md) — tracked open issues
- [Test & doc conventions](../.cursor/skills/feature-development/conventions.md) — assemblies, test style, doc template

---

## Role in the development cycle

Apply these guidelines at each phase:

| Phase | Action |
| --- | --- |
| **Design** | Draft or update the [integration spec](specs/README.md) for the element. Define public API, binding pattern, and events before coding. |
| **Test design** | Encode the public contract in Edit/Play Mode tests. See [conventions](../.cursor/skills/feature-development/conventions.md). |
| **Implementation** | Follow validation, lifecycle, and API rules below. Map each test to code. |
| **Review** | Run the [checklist](#checklist-before-merge) and compare against the spec and integration review open points. |
| **Documentation** | Update component/panel reference docs and the integration spec. Index in [README](README.md). |

---

## Element types

| Type | Location | Responsibility | Examples |
| --- | --- | --- | --- |
| **Component** | `Runtime/Scripts/Components/` | Single UI unit: display, input, or interaction | `IconAmount`, `RoButton`, `DropZone` |
| **Panel** | `Runtime/Scripts/Panels/` | Layout, collection, or container over multiple children | `ListPanel`, `FillPanel`, `Window` |
| **Wrapper** | Either | Thin subclass of Unity/TMP type with package styling only | `RoInput`, `RoToggle`, `RoScrollPanel` |

**Rule of thumb:** If external code binds *data* to it or subscribes to *collection-level* events, it is a panel or a display component with a binding API. If it is primarily a styled Unity control, it is a wrapper.

---

## Target integration pattern

External host code should interact through **public APIs and events**, not prefab internals.

```csharp
// Display / collection binding
panel.Clear();
panel.AddItems(data, (row, item) => row.Bind(item));

// Interaction
row.OnItemActivated.AddListener(() => host.OnRowChosen(row));
slider.onValueChanged.AddListener(host.OnVolumeChanged);
```

### Anti-pattern (do not require this from consumers)

```csharp
// Reaching into children — fragile, breaks when prefab structure changes
var label = row.GetComponentInChildren<TextMeshProUGUI>();
label.text = entry.Name;
```

Every display component and data-bearing panel should make the desired pattern explicit in its [integration spec](specs/README.md).

---

## Public API design

### Principles

1. **Surface what hosts need** — properties and methods for displayed data, visibility, interactable state, and subscription points.
2. **One-call binding** — prefer `Bind(...)`, `Configure(...)`, `Assign(...)`, or `Set(sprite, text)` over multiple loose setters when fields are always updated together.
3. **Presentation is centralized** — visibility, child active state, and formatting rules live in one refresh path (e.g. `RefreshPresentation()`), called from all public mutators.
4. **Events for output, methods for input** — hosts set data via methods/properties; user actions flow out through `UnityEvent` or typed callbacks.
5. **Read access matches write access** — panels expose `Count`, `GetItem(int)`, `GetCell(int)` when hosts need to query state after binding.
6. **Conservative defaults** — virtual methods like `CanDrop` / `Drop` should default to `false` or be clearly documented; permissive defaults are easy to forget to override.
7. **No dead API** — remove unused events and properties, or wire them. Do not declare `onSkillLevelUp` if it is never invoked.
8. **Encapsulation** — expose events via properties (`OnSwapped { get; }`), not only private serialized fields. Avoid public mutable fields (`activeGroup`) when a method should be the sole mutator.

### API shape by category

| Category | Preferred surface | Example |
| --- | --- | --- |
| **Display** | `Assign`, `Clear`, `SetActive`, typed properties | `IconAmount.Assign(sprite, amount)` |
| **List row** | `Bind(data)` on subclass or virtual method | `InventoryRow.Bind(entry)` |
| **Collection panel** | `Clear`, `AddItems<T>(data, bind)`, read API, panel events | `ListPanel.AddItems(...)` |
| **Grid panel** | `SetFilledCells`, `Refresh`, `GetCell` / `ForEachCell` | `FillPanel.ForEachCell(...)` |
| **Input** | Inherited Unity API + optional `Label` helper | `RoButton.onClick`, `confirmButton.Label` |
| **Interaction** | Virtual hooks + outcome events with useful args | `DropZone.CanDrop(payload)`, `OnDropAccepted(payload)` |

### Property setters

Property setters must follow the same contract as methods:

- Call `EnsureReferences()` before touching serialized fields.
- Route through the presentation refresh path when visibility or formatting rules apply.
- If a setter intentionally bypasses policy (editor-only), document that in the integration spec.

---

## Reference validation (`EnsureReferences`)

`IComponent` and `IPanel` define a single shared contract:

```csharp
bool EnsureReferences();
```

### Rules

| Do | Don't |
| --- | --- |
| Return `true` when all required references are present | Throw `NotImplementedException` or any exception |
| Return `false` and log via `Tools.LogMissingReference` when required refs are missing | Return inverted logic (valid ref → `false`) |
| Call `EnsureReferences()` at the start of every public method that uses serialized refs | Dereference serialized fields in `Update` without a guard |
| Disable the component or skip input handlers when validation fails | Continue running per-frame logic that will null-ref |
| Validate all refs used in `Awake` / `Start` / `OnEnable`, not only the happy path | Validate only the fields used in one code path |

### After failed validation

```csharp
private bool referencesValid;

private void Awake()
{
    referencesValid = EnsureReferences();
    if (!referencesValid)
        enabled = false;
}

public void SetText(string value)
{
    if (!referencesValid) return;
    // ...
}
```

For input components (`RoSlider`, `Resizable`, drag handlers), guard **all** pointer/drag entry points — not only `OnDrag`.

### `IPanel` / `IComponent` adoption

- Implement `IPanel` on panels that own layout and serialized child refs.
- Implement `IComponent` when reference validation adds value beyond a thin Unity wrapper.
- **Do not** implement the interface until `EnsureReferences()` is fully implemented. A stub that throws breaks any caller that treats the type as a valid contract.

---

## Initialization and lifecycle

### Recommended order

| Method | Use for |
| --- | --- |
| `Awake` | `EnsureReferences()`, cache sibling/component refs, one-time setup that does not register listeners |
| `OnEnable` | Register `UnityEvent` / button listeners |
| `OnDisable` | Unregister listeners; cancel in-flight interaction safely |
| `Start` | Deferred setup that depends on rest of hierarchy (e.g. `ListPanel` item registration) |

### Do

- Register listeners in **one** lifecycle method (`OnEnable`), unregister in `OnDisable`.
- Use `Awake` only for reference validation and caching — not for manually calling `OnEnable()`.
- Document ownership when `Clear()` destroys instantiated children.
- Prefer `OnRectTransformDimensionsChange` over per-frame polling for layout-driven grids.
- Finish or cancel drag/resize sessions consistently (same pointer button filter at begin and during drag).

### Don't

- Call `OnEnable()` manually from `Awake` — Unity invokes `OnEnable` separately → duplicate listeners.
- Leave `Update` running on misconfigured components.
- Run `Debug.Log` on hot paths (e.g. every item activation) without editor/debug gating.
- Instantiate children in `Awake` without exposing accessors for host binding afterward.

---

## Data binding and presentation

### Display components (`IconAmount`, `IconText`, `ItemLine`, …)

- Expose **`Assign` / `Clear` / `Set(...)`** as the primary integration surface.
- Encode presentation rules in tests (e.g. amount `1` hides text, `Clear()` deactivates children and sets `IsVisible = false`).
- Support both numeric and string labels when consumers need them (`Assign(Sprite, int)` and `Assign(Sprite, string)` or unified `SetPresentation`).

### Collection panels (`ListPanel`, `SkillMinPanel`, …)

- Provide **`Clear` + `AddItems<T>(data, bindAction)`** mirroring the list pattern.
- Accept a template parameter or use a serialized default template.
- Expose panel-level **`OnItemFocused` / `OnItemActivated`** (or equivalent) so hosts do not wire every row individually.
- Document whether `Clear()` destroys GameObjects; consider `Clear(destroyItems: bool)` for pooling.

### Grid panels (`FillPanel`, `GearPanel`, …)

- After layout (`Refresh`, `InitializePanels`), expose **typed cell/slot access**: `GetCell(int)`, `IReadOnlyList<T>`, or `ConfigureGear(index, ...)`.
- Prefer typed cell templates (`IconAmount`, `DraggableItem`) over raw `GameObject` when structure is fixed.

### Rows and items (`ListItem`, `SkillEntry`, …)

- Subclass or configure with serialized child components; expose **`Bind` / `Configure`** that delegates to children.
- Decouple from concrete panel types via **`IListItemHost`** (or similar) when reuse outside one panel is needed.
- Enforce UI state from flags (`IsFixedLevel` → disable buttons; `IsPassive` → adjust cost display).

---

## Events and host integration

| Do | Don't |
| --- | --- |
| Pass **useful context** in events (`DragPayload`, `ListItem`, index) | Force hosts to re-query state from partial event args |
| Expose `UnityEvent` through public properties for code subscription | Rely only on Inspector wiring for host logic |
| Remove listeners before re-binding (`RemoveAllListeners` or persistent handler pattern) | Accumulate duplicate listeners on each refresh |
| Document when to use custom events vs inherited Unity events (`onValueChanged` vs `onDecreaseClick`) | Duplicate semantics without documentation |
| Notify on cancellation (drag disabled mid-drag, panel cleared) when hosts depend on completion | Silently abort interaction without document or event |

---

## Namespaces, naming, and file layout

| Do | Don't |
| --- | --- |
| Use `RO_Flex_UI.Components` for components, `RO_Flex_UI.Panels` for panels | Place types in `global` namespace |
| Match **file name to class name** (`FillPanel.cs` → `FillPanel`) | Ship `FillPanel.cs` containing `FillPanel2` |
| Align folder, namespace, and doc category | Put panel classes in `Components` namespace under `Panels/` folder |
| Name methods for behavior (`SetSelected`, `SwapToNextGroup`) | Use names that collide with Unity API (`SetActive` on non-`GameObject`) |
| Use `RoButton` for package buttons in new work | Mix raw `Button` without documented reason |

---

## Wrappers (`RoInput`, `RoToggle`, `RoDropdown`, `RoScrollPanel`)

Thin wrappers are acceptable when:

- The value is consistent package styling and prefab anchoring.
- Integration is entirely through the inherited Unity/TMP API.
- The type is documented as a wrapper in its integration spec and reference doc.

Add `IComponent` and `EnsureReferences()` only when missing refs cause runtime failures that validation can prevent.

Optional helpers (`Label` on `RoButton`) should be added when prefab structure is consistent across the package.

---

## Testing requirements

Follow [conventions](../.cursor/skills/feature-development/conventions.md).

### Test the public contract

- Happy path binding (`Assign` → visible content, correct text rules).
- Clear / empty states (`Clear`, null sprite, zero amount).
- Validation failure (missing refs → public methods no-op, no throw).
- Lifecycle edge cases (duplicate listener registration, disable mid-drag).
- Panel population (`AddItems`, `Clear`, count after operations).

### Choose Edit vs Play Mode

| Edit Mode | Play Mode |
| --- | --- |
| API, binding, validation, collection logic | Pointer/drag, coroutines, EventSystem, per-frame behavior |

### Do not test

- Unity engine guarantees (e.g. `Button.interactable` round-trip with no custom logic).
- Implementation details (private field values) when public behavior is already covered.

---

## Documentation requirements

Each component or panel needs:

1. **Integration spec** (`Documentation~/specs/...`) — purpose, current public interface, **desired integration pattern**, related scenarios.
2. **Reference doc** (`Documentation~/Components/...` or `Documentation~/Panels/...`) — description, displayed data table, public API table, examples.
3. **Index entry** in [Documentation~/README.md](README.md).

When changing public API, update the spec first (or in the same change), and add an entry to [integration review open points](integration-review.md#open-points-from-specs) if implementation lags the spec.

---

## Multi-element scenarios

When a feature spans multiple types, read the relevant [scenario](integration-review.md#multi-element-scenarios) before designing API:

- Selectable list → `ListPanel` + `ListItem`
- Inventory grid → `FillPanel` + `IconAmount` + optional drag/drop
- Equipment → `GearPanel` + `IconText`
- Skill list → `SkillMinPanel` + `SkillEntry`
- Tooltips → `TooltipPanel` + `Tooltip`
- Tabs → `TabsPanel` + `TabButton`
- Windows → `Window` + `Header` + `Draggable` / `Resizable`

Design each type so the scenario code reads as a short, linear host script — no child lookups.

---

## Checklist before merge

Use this for new elements and substantive changes to existing ones.

### API and integration

- [ ] Integration spec exists or is updated with desired binding pattern
- [ ] Host can bind data without `GetComponentInChildren` or reflection
- [ ] Batch/binding method provided where multiple fields update together
- [ ] Events expose enough context for host handlers
- [ ] No dead or throwing public members

### Validation and lifecycle

- [ ] `EnsureReferences()` returns `bool`, never throws
- [ ] All public methods guard on validation state
- [ ] Listeners registered once (`OnEnable` / `OnDisable` pairing)
- [ ] Per-frame and input paths safe when refs are missing
- [ ] Serialized constraints enforced in setters (`[Min]`, clamping)

### Quality

- [ ] Tests cover public contract and key edge cases
- [ ] Reference doc and spec updated
- [ ] Namespace, file name, and folder aligned
- [ ] No hot-path `Debug.Log` in production integrations
- [ ] Open point in integration review resolved or tracked

---

## Do's and Don'ts (summary)

### Do

- Design API from the **host's perspective** first.
- Centralize presentation and visibility rules.
- Write the integration spec before implementation.
- Test behavior consumers rely on, not internals.
- Expose read APIs on panels (`Count`, `GetItem`, `GetCell`).
- Decouple rows from concrete panels when reuse matters.
- Disable or guard components that fail validation.
- Use typed templates and accessors for grid/slot content.
- Document ownership semantics for `Clear()` and instantiation.

### Don't

- Throw from `EnsureReferences()` or leave it unimplemented while implementing `IPanel` / `IComponent`.
- Require `GetComponentInChildren` for routine data binding.
- Split presentation rules across direct property sets and methods without refresh.
- Manually invoke Unity lifecycle methods (`OnEnable` from `Awake`).
- Ship debug stubs (`ScrollPanel` with `Debug.Log`) as production API.
- Leave flags (`IsFixedLevel`) that do not affect UI or handlers.
- Use global namespace for package types.
- Default virtual permission methods to `true` without strong documentation.
- Add `IComponent` / `IPanel` as a marker without real validation.

---

## Adding a new UI element (workflow)

1. **Spec** — Add `Documentation~/specs/Components/your-element.md` or `Panels/...` using an existing spec as template.
2. **Scenario** — If the element participates in a multi-element flow, link to or add a scenario in [integration-review.md](integration-review.md).
3. **Test design** — Propose test cases (feature development skill, steps 1–2).
4. **Plan** — Map tests to files and API shape; get developer approval.
5. **Implement** — Apply this guideline document.
6. **Document** — Reference doc + README index + update spec "current public interface" section.
7. **Review** — Run checklist; remove or update integration review open point.

---

[Back to documentation index](README.md)
