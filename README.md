# ERLoader

ERLoader is the bootstrap repository for a unified Elden Ring mod launcher that combines:

- Mod Engine 2 file-based mod loading
- Elden Mod Loader–style DLL mod loading semantics
- one canonical TOML configuration model
- one Windows desktop launcher UI

## Project status

This repository is in the initial bootstrap phase.

The current priority is to establish:
1. clean repository structure
2. documented architecture decisions
3. imported upstream reference code boundaries
4. a stable config/profile model
5. a WPF launcher shell for the future GUI

## Planned repository layout

```text
ERLoader/
├── docs/
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
└── build/
```

## Architecture direction

- `vendor/modengine2/` will hold the imported Mod Engine 2 baseline.
- `vendor/elden-mod-loader-reference/` will hold legacy reference code used to preserve behavior expectations.
- `src/ERLoader.Backend/` will contain ERLoader-owned backend/runtime integration.
- `src/ERLoader.LoaderCompat/` will contain DLL compatibility behavior.
- `src/ERLoader.Config/` will own the canonical TOML profile model.
- `ui/ERLoader.Wpf/` will contain the Windows-first launcher UI.

## Near-term milestones

1. Bootstrap the repository structure and docs
2. Import upstream baselines under `vendor/`
3. Define the config/profile schema
4. Build the WPF launcher shell
5. Integrate runtime launch orchestration
6. Add import, validation, logging, and recovery flows

## Prototype status

A downloadable Windows prototype now exists under:
- `ERLoader.sln`
- `ui/ERLoader.Wpf/`

Current prototype capabilities:
- unified launcher shell
- profile switching
- file mod list
- DLL mod list with load order / delay columns
- validation view with blockers and warnings
- logs view
- safe recovery action
- mock launch flow for UX testing

## Build the prototype on Windows

1. Open `ERLoader.sln` in Visual Studio 2022.
2. Let NuGet restore packages.
3. Build and run the `ERLoader.Wpf` project.

See `ui/ERLoader.Wpf/README.md` for more detail.

## Notes

- Elden Ring only for v1
- Portable zip distribution target for v1
- Advanced options hidden by default
- Safe/vanilla recovery mode is a required product behavior
