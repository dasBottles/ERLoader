# ADR 0001: Initial Repository Layout

## Status
Accepted

## Context

The `ERLoader` repository started effectively empty, containing only `LICENSE`.

The project needs to combine:
- imported upstream code
- new backend/runtime integration
- config/domain logic
- a Windows desktop UI
- tests and documentation

Without a clear repository layout, imported code and ERLoader-owned logic would be easy to mix together, making later maintenance harder.

## Decision

Use the following initial repository layout:

```text
ERLoader/
├── docs/
│   ├── plans/
│   ├── architecture/
│   └── decisions/
├── vendor/
│   ├── modengine2/
│   └── elden-mod-loader-reference/
├── src/
│   ├── ERLoader.Backend/
│   ├── ERLoader.LoaderCompat/
│   ├── ERLoader.Config/
│   └── ERLoader.Launcher/
├── ui/
│   └── ERLoader.Wpf/
├── tests/
│   ├── config/
│   ├── import/
│   └── validation/
└── build/
```

## Rationale

- `vendor/` isolates third-party imported baselines.
- `src/` holds ERLoader-owned runtime and domain code.
- `ui/` gives the WPF launcher a dedicated home.
- `tests/` keeps config, import, and validation tests visible early.
- `docs/` captures plans, architecture notes, and decisions from the start.
- `build/` provides a stable place for generated build artifacts that should not be committed.

## Consequences

### Positive
- Clean separation between imported and owned code
- Easier upstream inventory and future rebases
- Lower risk of architectural drift during bootstrap
- Documentation-first structure for early contributors

### Negative
- Slightly more up-front scaffolding work
- More directories than the current code volume strictly needs

## Follow-up decisions

- upstream import strategy
- solution/project layout for backend and WPF app
- TOML schema boundaries
- import and validation test strategy
