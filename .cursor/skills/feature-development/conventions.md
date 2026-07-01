# RO Flex UI Conventions

Project-specific patterns for test design and documentation. Read this when executing steps 1, 2, and 7 of the feature development workflow.

For API shape, validation, lifecycle, integration patterns, and the pre-merge checklist, see [UI Element Development Guidelines](../../../Documentation~/ui-element-guidelines.md).

## Testing

### Locations

| Mode | Path | Use when |
|------|------|----------|
| Edit Mode | `RO Flex UI/Tests/Edit Mode/` | API and logic without runtime input, drag, or coroutines |
| Play Mode | `RO Flex UI/Tests/Play Mode/` | Drag, pointer events, coroutine lifecycles |

Default to Edit Mode unless the feature depends on runtime input or frame timing.

### Assembly and Namespace

- Edit Mode assembly: `RO.FlexUI.Tests.EditMode`
- Play Mode assembly: `RO.FlexUI.Tests.PlayMode`
- Namespace: `RO_Flex_UI.Tests`

### Prefab Paths

Use `Setup.PrefabRoot` from `RO Flex UI/Tests/Edit Mode/Utils/Setup.cs`:

```
Packages/com.ricric.roflexui/Runtime/Prefabs/
```

### Example Tests

| File | Patterns to follow |
|------|-------------------|
| `RO Flex UI/Tests/Edit Mode/Components/RoButtonTests.cs` | Public API (`interactable`, `onClick`), prefab instantiation, `Object.DestroyImmediate` cleanup |
| `RO Flex UI/Tests/Edit Mode/Components/ListPanelTests.cs` | Fixture helpers, configured-order validation, null items, templates |
| `RO Flex UI/Tests/Play Mode/DropZonePlayModeTests.cs` | EventSystem/Canvas setup, drag-drop lifecycle, `[UnityTest]` |

### Naming and Assertions

- Test method names describe behavior: `InteractableCanBeChangedFromCode`, not `Test1`.
- Include descriptive failure messages in assertions.
- Prefer one focused assertion theme per test method.
- Clean up created GameObjects with `Object.DestroyImmediate` in Edit Mode tests.

### Meaningful Tests

**Do:**

- Test the public contract, edge cases, and integration between components
- Assert behavior that matters to consumers of the API

**Do not:**

- Test Unity or framework behavior already guaranteed by the engine
- Assert obvious defaults without behavioral significance
- Add tests that only verify a property round-trips without meaningful behavior

## Documentation

### Location

- Primary: `Documentation~/`, indexed from `Documentation~/README.md`
- Package copy: `RO Flex UI/Documentation/` — keep in sync when that path is the package-facing docs

When unsure which path is canonical, check which location already has the richest content for similar components.

### Page Template

Match the structure of `Documentation~/Components/ro-button.md`:

1. **Description** — What the component/panel does and key dependencies
2. **Displayed Data** — Table of data sources and what they show
3. **Public API** — Table of properties, methods, and events
4. **Examples** — C# snippets showing typical usage

### Index Updates

Add new component or panel links to `Documentation~/README.md` under the appropriate section (Components, Panels, Windows).

### Cross-References

When documenting a new feature, read similar existing docs and tests for tone, table format, and example style before writing.
