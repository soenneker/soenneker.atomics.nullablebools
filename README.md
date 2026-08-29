[![](https://img.shields.io/nuget/v/soenneker.atomics.nullablebools.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.atomics.nullablebools/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.atomics.nullablebools/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.atomics.nullablebools/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.atomics.nullablebools.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.atomics.nullablebools/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.atomics.nullablebools/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.atomics.nullablebools/actions/workflows/codeql.yml)

# Soenneker.Atomics.NullableBools

A lightweight atomic tri-state flag implemented on top of an inline `ValueAtomicInt`. Backing values: `-1` = null / unknown `0` = false `1` = true.

## Install

```bash
dotnet add package Soenneker.Atomics.NullableBools
```

## Usage

The null state is useful for a value that has not been determined yet. `TrySet` lets one caller publish the first result:

```csharp
using Soenneker.Atomics.NullableBools;

var availability = new AtomicNullableBool();

if (availability.TrySet(await ProbeAvailability()))
{
    // This caller published the transition from unknown to known.
}

bool? result = availability.Value;
availability.Reset();
```

`GetValueOrFalse` and `GetValueOrTrue` provide explicit fallback policies without changing the stored state.

Prefer the nullable `Value`, `Set`, `TrySet`, and `Reset` members in application code. `Read`, `Write`, and `TryCompareExchange` expose the raw representation (`-1`, `0`, `1`) and do not validate supplied integers; invalid raw values can produce misleading boolean and string results.

## What you get

- `AtomicNullableBool` — A lightweight atomic tri-state flag implemented on top of an inline `ValueAtomicInt`. Backing values: `-1` = null / unknown `0` = false `1` = true.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `AtomicNullableBool.HasValue` | Gets a value indicating whether the current state is non-null. | Gets a value indicating whether the current state is non-null. |
| `AtomicNullableBool.Value` | Gets or sets the current value as a nullable boolean. | Gets or sets the current value as a nullable boolean. |
| `AtomicNullableBool.Read()` | Reads the raw backing state. | `-1` (null), `0` (false), or `1` (true). |
| `AtomicNullableBool.GetValueOrFalse()` | Gets the value, treating `null`/`unknown` as `false`. | true if gets the value, treating null/unknown as; otherwise, false. |
| `AtomicNullableBool.GetValueOrTrue()` | Gets the value, treating `null`/`unknown` as `true`. | true if gets the value, treating null/unknown as; otherwise, false. |
| `AtomicNullableBool.Set(value)` | Sets the state to `true` or `false`. | Returns no value; the requested change is complete when the method returns. |
| `AtomicNullableBool.TrySet(value)` | Attempts to set the state to `true` or `false` only if the current state is `null`/`unknown`. | true if the requested update was applied; otherwise, false. |
| `AtomicNullableBool.TryCompareExchange(newState, expected)` | Attempts to transition the state from `expected` to `newState`. | true if the requested update was applied; otherwise, false. |
| `AtomicNullableBool.Reset()` | Resets the state to `null`/`unknown`. | Returns no value; the requested change is complete when the method returns. |
| `AtomicNullableBool.ToString()` | Returns a string representation of the current state. | Returns `string`. |

## Important behavior

- `AtomicNullableBool`: Reads establish acquire semantics and writes establish release semantics. This is a mutable reference type. Use as a private field and avoid exposing the instance publicly unless you want shared, aliasable state.
- `AtomicNullableBool.Write(state)`: Callers must only provide valid values: `-1`, `0`, or `1`.
