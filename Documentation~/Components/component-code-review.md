# Component Code Review Plan

Re-reviewed on 2026-06-25 against the current working tree for `RO Flex UI/Runtime/Scripts/Components`.

Scope: re-evaluation of the issues previously tracked in this file. Components and severity sections with no open issues were removed. No builds or tests were run for this documentation-only update.

## DraggableItem

### Medium

- `RO Flex UI/Runtime/Scripts/Components/DraggableItem.cs:106`: `OnDisable` still resolves an active drag with `FinishDrag(false, false)`, which suppresses `onDropRejected` and `onEndDrag`. Systems listening for drag completion can still be left waiting when a source item is disabled mid-drag. Prefer a single cancellation path that notifies listeners, or document that disable is silent and expose a separate cancellation event.

### Low

- `RO Flex UI/Runtime/Scripts/Components/DraggableItem.cs:173`: The amount-based path uses `IconAmount.Assign`, but the non-amount path writes `Sprite` and `Text` directly. That still bypasses any `IconAmount` visibility or active-state policy. Add an `IconAmount.Assign(Sprite, string)` style method or update visibility consistently after direct assignment.

## Header

### High

- `RO Flex UI/Runtime/Scripts/Components/Header.cs:14`: `EnsureReferences` still throws `NotImplementedException`. Any consumer treating `Header` as an `IComponent` will crash at runtime. Implement reference validation for `funButton`, `minButton`, `closeButton`, and `title`, or remove `IComponent` until the contract is supported.

### Low

- `RO Flex UI/Runtime/Scripts/Components/Header.cs:5`: `Header` still lives in the `RO_Flex_UI.Panels` namespace despite being stored under `Components`. This is not a runtime defect, but it makes menu generation, docs, and API discovery easy to misread. Align the namespace or move the file.

## IconAmount

### Medium

- `RO Flex UI/Runtime/Scripts/Components/IconAmount.cs:40`: `Assign` sets the sprite/text values but does not update `visible`, the root active state, or child image/text active states. Existing expectations in `DropZoneTests.IconAmountAssignsAndClearsPresentation` require assigned content to become visible and amount text to hide for amount `1`. Restore a consistent presentation refresh path.
- `RO Flex UI/Runtime/Scripts/Components/IconAmount.cs:70`: `Sprite` and `Text` setters still bypass the component's visibility policy. Code that updates these properties directly can leave stale active state. Route setters through assignment methods or refresh presentation state consistently.

### Low

- `RO Flex UI/Runtime/Scripts/Components/IconAmount.cs:35`: `ToggleText` still dereferences `iconText` without validating references. Guard with `EnsureReferences()` for consistency with the other public methods.

## IconText

### Low

- `RO Flex UI/Runtime/Scripts/Components/IconText.cs:52`: `Text` and `Sprite` accessors still dereference serialized fields directly. Other methods validate references, so partially wired prefabs fail inconsistently depending on API path. Either validate in accessors or document these as requiring configured references.

## ItemLine

### High

- `RO Flex UI/Runtime/Scripts/Components/ItemLine.cs:47`: `Setup` dereferences `socketTemplate` even though `EnsureReferences` only validates `text`. Any prefab missing `socketTemplate` will throw in `Awake`. Validate `socketTemplate` when it is required, or make `Setup` null-safe.

## ListItem

### Medium

- `RO Flex UI/Runtime/Scripts/Components/ListItem.cs:24`: `Awake` manually calls `OnEnable`, and Unity also invokes `OnEnable` for enabled components. That registers `HandleSingleClickFocus` twice on `TargetButton.onClick`, so a single click can fire focus logic twice. Replace the manual lifecycle call with `EnsureReferences()` or move listener setup to one lifecycle method.

## Resizable

### Medium

- `RO Flex UI/Runtime/Scripts/Components/Resizable.cs:107`: `[Min(1f)]` only guards Inspector edits; the public `StepSize` setter can still assign zero or negative components, and `OnDrag` divides by `stepSize` at line 71. Clamp in the setter, `OnValidate`, and/or before division.
- `RO Flex UI/Runtime/Scripts/Components/Resizable.cs:45`: `OnBeginDrag` still does not filter for the left button, but `OnDrag` does. Right or middle button drags can still emit `onBeginResize` and later `onEndResize` without a resize. Apply the same button gate at drag start.

### Low

- `RO Flex UI/Runtime/Scripts/Components/Resizable.cs:34`: `EnsureReferences` still requires `targetTransform` to be assigned instead of defaulting to the current `RectTransform`, unlike `DraggableBase`. If this is intentional, document it; otherwise defaulting would reduce prefab setup errors.

## RoSlider

### Medium

- `RO Flex UI/Runtime/Scripts/Components/RoSlider.cs:133`: Pointer handling can still reach `IsPointerInsideDragArea` even after `EnsureReferences` failed in `Awake`. A missing `dragArea` should disable drag handling or be guarded at use sites so a misconfigured prefab cannot throw during input.

## SkillEntry

### High

- `RO Flex UI/Runtime/Scripts/Components/SkillEntry.cs:31`: `EnsureReferences` now checks `skillNameText`, but the condition is inverted: a valid `skillNameText` makes the method return `false`, while a missing one can continue and potentially return `true`. Fix the validation so missing references fail and valid references pass.
- `RO Flex UI/Runtime/Scripts/Components/SkillEntry.cs:56`: `OnEnable` still dereferences `skillLevelUp` and `skillLevelDown` without guarding the result of `EnsureReferences`. Missing button references can still crash as soon as the object is enabled. Validate before listener registration and skip registration when validation fails.

### Medium

- `RO Flex UI/Runtime/Scripts/Components/SkillEntry.cs:111`: `IsFixedLevel` is still stored but never applied to the up/down buttons or handlers. Fixed-level skills can still emit increase/decrease events. Disable the buttons or block handlers while fixed.

### Low

- `RO Flex UI/Runtime/Scripts/Components/SkillEntry.cs:21`: `onSkillLevelUp` is still declared but never invoked. If this is obsolete, remove it; otherwise wire it into the level-up flow.

## ToggleSwitch

### Medium

- `RO Flex UI/Runtime/Scripts/Components/ToggleSwitch.cs:106`: `OnMove` is still overridden as an empty method, which blocks inherited `Slider` keyboard/gamepad navigation without replacing it with a toggle action. Keyboard/controller users cannot change the control through normal navigation. Handle submit/move explicitly or preserve inherited behavior where appropriate.

### Low

- `RO Flex UI/Runtime/Scripts/Components/ToggleSwitch.cs:28`: `IsOn` still reflects the animated slider value, not `targetState`. During animation it can report the old state until the value reaches exactly `maxValue`. If callers need logical state, expose `CommittedState`/`TargetState` or make `IsOn` return the committed state.

## Tooltip

### Medium

- `RO Flex UI/Runtime/Scripts/Components/Tooltip.cs:48`: `Update` still dereferences `background` every frame without checking whether `EnsureReferences` succeeded. A misconfigured tooltip logs a missing reference in `Start` and then continues into repeated null-reference exceptions. Disable the component on failed validation or guard `Update`.
- `RO Flex UI/Runtime/Scripts/Components/Tooltip.cs:53`: `SetText`, `ShowTooltip`, and `RefreshSize` still dereference `text` and `background` without validation. External callers can hit null-reference exceptions even though the component has an `EnsureReferences` contract. Guard public methods or cache validation state.

### Low

- `RO Flex UI/Runtime/Scripts/Components/Tooltip.cs:6`: `Tooltip` is still declared in the global namespace while most package components use `RO_Flex_UI.Components`. This increases collision risk with other packages and makes API discovery inconsistent. Move it into the component namespace and update serialized references carefully.

## TooltipPanel

### Low

- `RO Flex UI/Runtime/Scripts/Components/TooltipPanel.cs:12`: `iconAmount` is still cached only in `Awake`. If an `IconAmount` is added dynamically after `Awake`, tooltip visibility ignores it. This is acceptable for prefab-only usage, but dynamic composition should use lazy lookup or explicit assignment.

## Removed As Addressed Or No Open Issue

- `Draggable`
- `DraggableBase`
- `DropZone`
- `IComponent`
- `IDropZone`
- `InventoryTabGroup`
- `RoButton`
- `RoDropdown`
- `RoInput`
- `RoToggle`
- `RoSlider` step-size calculation finding
- `ListItem` initial double-click and mouse-button findings
- `Resizable` serialized Inspector minimum finding, except for the still-open public setter path
