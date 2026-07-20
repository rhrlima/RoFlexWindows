# SkillEntry

[Go Back](../README.md)

## Description

`SkillEntry` displays a skill name, level, and cost with controls for requesting
level changes. It forwards button clicks as UnityEvents; application code owns
the actual skill-level and cost calculations.

## Displayed Data

| Data | Type | Description |
| --- | --- | --- |
| `Name` | `string` | Skill name label. |
| `Level` | `string` | Skill level label. |
| `Cost` | `string` | Cost label, or `Passive` while `IsPassive` is true. |

## Public API

### Properties

| Property | Type | Description |
| --- | --- | --- |
| `Name` | `string` | Gets or replaces the name label. |
| `Level` | `string` | Gets or replaces the level label. |
| `Cost` | `string` | Gets or replaces the cost label, respecting passive state. |
| `IsPassive` | `bool` | Controls passive cost display behavior. |
| `IsFixedLevel` | `bool` | Stores whether the represented skill has a fixed level. |

### Events

| Event | Description |
| --- | --- |
| `onIncreaseLevel` | Invoked by the level-up button. |
| `onDecreaseLevel` | Invoked by the level-down button. |
| `onSkillLevelUp` | Public event available for application wiring; it is not invoked internally. |

## Examples

```csharp
entry.Name = skill.Name;
entry.Level = skill.Level.ToString();
entry.Cost = skill.Cost.ToString();
entry.onIncreaseLevel.AddListener(() => TryIncrease(skill));
```

[Go Back](../README.md)
