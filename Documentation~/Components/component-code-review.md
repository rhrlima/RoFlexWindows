# Component Code Review

Reviewed current working tree on 2026-06-24 for `RO Flex UI/Runtime/Scripts/Components`.

Scope: component source files only. Related tests, generated menu references, prefabs/scenes, and direct call sites were sampled for context. No builds or test runs were executed for this documentation-only review.

Workspace note: the working tree already contained uncommitted changes in `ListItem.cs` and `ROSlider.cs`; this review covers the current file contents.

## Draggable

### High

No high-severity findings.

### Medium

No medium-severity findings.

### Low

No low-severity findings. `Draggable` is only a compatibility/convenience subclass of `DraggableBase`.

## DraggableBase

### High

No high-severity findings.

### Medium

- `RO Flex UI/Runtime/Scripts/Components/DraggableBase.cs:85`: `DraggableBase` has no `OnDisable` cleanup. If the component or GameObject is disabled while a drag is active, `dragging` can remain true and `onEndDrag` is never emitted. Add an `OnDisable` path that clears `dragging`, optionally restores origin when `returnToOrigin` is enabled, and emits or documents the cancellation behavior.

### Low

- `RO Flex UI/Runtime/Scripts/Components/DraggableBase.cs:91`: `OnEndDrag` returns before invoking `onEndDrag` when `targetTransform` is null. If the target is destroyed or cleared during a drag, listeners do not receive a terminal event. Move the event invocation outside the target check or emit a separate cancellation signal.

## DraggableItem

### High

No high-severity findings.

### Medium

- `RO Flex UI/Runtime/Scripts/Components/DraggableItem.cs:106`: `OnDisable` resolves an active drag with `FinishDrag(false, false)`, which suppresses `onDropRejected` and `onEndDrag`. Systems listening for drag completion can be left waiting when a source item is disabled mid-drag. Prefer a single cancellation path that notifies listeners, or document that disable is silent and expose a separate cancellation event.

### Low

- `RO Flex UI/Runtime/Scripts/Components/DraggableItem.cs:173`: The amount-based path uses `IconAmount.Assign`, but the non-amount path writes `Sprite` and `Text` directly. That bypasses `IconAmount` presentation state and can leave the proxy text visible for sprites with empty text. Add a small `IconAmount.Assign(Sprite, string)` style method or update visibility after direct assignment.

## DropZone

### High

No high-severity findings.

### Medium

No medium-severity findings.

### Low

No low-severity findings. Existing edit-mode and play-mode tests cover accepted, rejected, failed, and duplicate drop resolution paths.

## Header

### High

- `RO Flex UI/Runtime/Scripts/Components/Header.cs:14`: `EnsureReferences` throws `NotImplementedException`. Any consumer treating `Header` as an `IComponent` will crash at runtime. Implement reference validation for `funButton`, `minButton`, `closeButton`, and `title`, or remove `IComponent` until the contract is supported.

### Medium

No medium-severity findings.

### Low

- `RO Flex UI/Runtime/Scripts/Components/Header.cs:5`: `Header` lives in the `RO_Flex_UI.Panels` namespace despite being stored under `Components`. This is not a runtime defect, but it makes menu generation, docs, and API discovery easy to misread. Align the namespace or move the file.

## IComponent

### High

No high-severity findings.

### Medium

No medium-severity findings.

### Low

No low-severity findings. The interface is small and clear.

## IconAmount

### High

No high-severity findings.

### Medium

- `RO Flex UI/Runtime/Scripts/Components/IconAmount.cs:104`: `Sprite` and `Text` setters bypass `hasPresentationState`, `hasContent`, `showAmount`, and `RefreshVisibility`. Code that updates these properties directly can display stale visibility state or show an empty amount label. Route setters through assignment methods or refresh presentation state consistently.

### Low

- `RO Flex UI/Runtime/Scripts/Components/IconAmount.cs:44`: `ToggleText` dereferences `iconText` without validating references. This is inconsistent with the rest of the public methods and will throw on a partially wired prefab. Guard with `EnsureReferences()`.

## IconText

### High

No high-severity findings.

### Medium

No medium-severity findings.

### Low

- `RO Flex UI/Runtime/Scripts/Components/IconText.cs:54`: `Text` and `Sprite` accessors dereference serialized fields directly. Other methods validate references, so partially wired prefabs fail inconsistently depending on API path. Either validate in accessors or document these as requiring configured references.

## IDropZone

### High

No high-severity findings.

### Medium

No medium-severity findings.

### Low

No low-severity findings. The interface matches `DraggableItem.TryDrop` usage.

## InventoryTabGroup

### High

No high-severity findings.

### Medium

No medium-severity findings.

### Low

No low-severity findings. The class currently only preserves a named specialization over `TabGroup`.

## ItemLine

### High

- `RO Flex UI/Runtime/Scripts/Components/ItemLine.cs:41`: `EnsureReferences` throws `NotImplementedException`. Any `IComponent` validation or external setup call will crash. Implement validation for the text and socket modes.
- `RO Flex UI/Runtime/Scripts/Components/ItemLine.cs:22`: `Awake` checks `socketsContainer` but immediately calls `socketTemplate.SetActive(false)` without confirming `socketTemplate` is assigned. A prefab with only one socket reference missing will throw during initialization.
- `RO Flex UI/Runtime/Scripts/Components/ItemLine.cs:48`: `SetText` dereferences `textObj` without validation. Text-mode entries crash on startup when `textObj` is not assigned.

### Medium

- `RO Flex UI/Runtime/Scripts/Components/ItemLine.cs:57`: `SetNumSockets` only instantiates more sockets and never clears previously generated socket children. Rebinding or changing counts at runtime duplicates sockets and can show stale state. Clear generated children or pool and reconcile to the requested count.
- `RO Flex UI/Runtime/Scripts/Components/ItemLine.cs:62`: Closed sockets never receive `closedSocket`; only open sockets get `openSocket`. If the template is not already configured with the closed sprite, closed slots render incorrectly. Assign both branches explicitly.

### Low

No low-severity findings beyond the items above.

## ListItem

### High

No high-severity findings.

### Medium

- `RO Flex UI/Runtime/Scripts/Components/ListItem.cs:67`: `lastClickTime` defaults to `0`, so the first click within `doubleClickLimit` seconds after scene load can activate the item as if it were a double-click. Initialize to a negative sentinel or track whether a previous click exists.
- `RO Flex UI/Runtime/Scripts/Components/ListItem.cs:65`: `OnPointerClick` does not filter mouse button. Two quick right-clicks or middle-clicks can activate a list item. Check `eventData.button == PointerEventData.InputButton.Left` before double-click activation.

### Low

- `RO Flex UI/Runtime/Scripts/Components/ListItem.cs:29`: `EnsureButtonCached` assumes `GetComponent<RoButton>()` succeeds. `[RequireComponent]` helps in editor/add-component flows, but existing broken prefab data or runtime removal would still throw at `TargetButton.onClick`. A null guard would make the component fail predictably.

## Resizable

### High

No high-severity findings.

### Medium

- `RO Flex UI/Runtime/Scripts/Components/Resizable.cs:71`: When `snapToStep` is enabled, `stepSize` is used as a divisor without enforcing positive components. A zero or negative step from the Inspector can produce invalid sizes or unstable snapping. Clamp `stepSize` in `OnValidate` and before division.
- `RO Flex UI/Runtime/Scripts/Components/Resizable.cs:45`: `OnBeginDrag` does not filter for the left button, but `OnDrag` does. Right or middle button drags can still emit `onBeginResize` and later `onEndResize` without a resize. Apply the same button gate at drag start.

### Low

- `RO Flex UI/Runtime/Scripts/Components/Resizable.cs:34`: `EnsureReferences` requires `targetTransform` to be assigned instead of defaulting to the current `RectTransform`, unlike `DraggableBase`. If this is intentional, document it; otherwise defaulting would reduce prefab setup errors.

## RoButton

### High

No high-severity findings.

### Medium

No medium-severity findings.

### Low

No low-severity findings. The thin wrapper behavior is documented and tested.

## RoDropdown

### High

No high-severity findings.

### Medium

No medium-severity findings.

### Low

No low-severity findings. The class is a thin `TMP_Dropdown` wrapper.

## RoInput

### High

No high-severity findings.

### Medium

No medium-severity findings.

### Low

No low-severity findings. The class is a thin `TMP_InputField` wrapper.

## RoSlider

### High

No high-severity findings.

### Medium

- `RO Flex UI/Runtime/Scripts/Components/RoSlider.cs:127`: `GetStepSize` returns `maxValue * stepSize`, which is wrong when `minValue` is not zero. For a slider range of `50..100`, a `0.2` step becomes `20` instead of `10`. Use `(maxValue - minValue) * stepSize`, or rename the field if it is meant to be a max-value fraction.
- `RO Flex UI/Runtime/Scripts/Components/RoSlider.cs:132`: Pointer handling can reach `IsPointerInsideDragArea` even after `EnsureReferences` failed in `Start`. A missing `dragArea` should disable drag handling or be guarded at use sites so a misconfigured prefab cannot throw during input.

### Low

- `RO Flex UI/Runtime/Scripts/Components/RoSlider.cs:83`: `OnEnable` registers button listeners before `Start` validates references. This is safe because the code null-checks buttons, but it means missing references are silent until `Start`. Consider moving validation/caching into `Awake` or `OnEnable` for earlier feedback.

## RoToggle

### High

No high-severity findings.

### Medium

No medium-severity findings.

### Low

No low-severity findings. The class is a thin `Toggle` wrapper.

## SkillEntry

### High

- `RO Flex UI/Runtime/Scripts/Components/SkillEntry.cs:33`: `OnEnable` dereferences `skillLevelUp` and `skillLevelDown` before any meaningful validation. Missing button references crash as soon as the object is enabled. Guard these fields and make `EnsureReferences` validate all serialized references.
- `RO Flex UI/Runtime/Scripts/Components/SkillEntry.cs:28`: `EnsureReferences` always returns `true`, so `Start` gives false confidence and never catches missing `TextMeshProUGUI` or button references. Implement validation or remove the interface.

### Medium

- `RO Flex UI/Runtime/Scripts/Components/SkillEntry.cs:88`: `IsFixedLevel` is stored but never applied to the up/down buttons or handlers. Fixed-level skills can still emit increase/decrease events. Disable the buttons or block handlers while fixed.

### Low

- `RO Flex UI/Runtime/Scripts/Components/SkillEntry.cs:20`: `onSkillLevelUp` is declared but never invoked. If this is obsolete, remove it; otherwise wire it into the level-up flow.

## ToggleSwitch

### High

No high-severity findings.

### Medium

- `RO Flex UI/Runtime/Scripts/Components/ToggleSwitch.cs:106`: `OnMove` is overridden as an empty method, which blocks inherited `Slider` keyboard/gamepad navigation without replacing it with a toggle action. Keyboard/controller users cannot change the control through normal navigation. Handle submit/move explicitly or preserve the inherited behavior where appropriate.

### Low

- `RO Flex UI/Runtime/Scripts/Components/ToggleSwitch.cs:28`: `IsOn` reflects the animated slider value, not `targetState`. During animation it can report the old state until the value reaches exactly `maxValue`. If callers need logical state, expose `CommittedState`/`TargetState` or make `IsOn` return the committed state.

## Tooltip

### High

No high-severity findings.

### Medium

- `RO Flex UI/Runtime/Scripts/Components/Tooltip.cs:48`: `Update` dereferences `background` every frame without checking whether `EnsureReferences` succeeded. A misconfigured tooltip logs a missing reference in `Start` and then continues into repeated null-reference exceptions. Disable the component on failed validation or guard `Update`.
- `RO Flex UI/Runtime/Scripts/Components/Tooltip.cs:53`: `SetText`, `ShowTooltip`, and `RefreshSize` dereference `text` and `background` without validation. External callers can hit null-reference exceptions even though the component has an `EnsureReferences` contract. Guard public methods or cache validation state.

### Low

- `RO Flex UI/Runtime/Scripts/Components/Tooltip.cs:6`: `Tooltip` is declared in the global namespace while most package components use `RO_Flex_UI.Components`. This increases collision risk with other packages and makes API discovery inconsistent. Move it into the component namespace and update serialized references carefully.

## TooltipPanel

### High

No high-severity findings.

### Medium

No medium-severity findings.

### Low

- `RO Flex UI/Runtime/Scripts/Components/TooltipPanel.cs:12`: `iconAmount` is cached only in `Awake`. If an `IconAmount` is added dynamically after `Awake`, tooltip visibility ignores it. This is acceptable for prefab-only usage, but dynamic composition should use lazy lookup or explicit assignment.

## Coverage Gaps

- `DraggableTests.cs` contains only commented-out tests, so `DraggableBase` behavior is effectively untested.
- There are focused tests for `DropZone`, `DraggableItem`, `IconAmount`, `RoButton`, and `ToggleSwitch`.
- No direct tests were found for `Header`, `IconText`, `ItemLine`, `ListItem` click semantics, `Resizable`, `RoDropdown`, `RoInput`, `RoSlider`, `RoToggle`, `SkillEntry`, `Tooltip`, or `TooltipPanel`.
