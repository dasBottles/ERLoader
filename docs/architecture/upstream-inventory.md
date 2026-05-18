# Upstream Inventory

This document records the upstream codebases inspected for ERLoader bootstrap and how they are used.

## 1. Mod Engine 2

- upstream repository: `https://github.com/soulsmods/ModEngine2`
- inspected local path: `/tmp/ModEngine2`
- inspected commit: `4f701ccd0e2f6ddb106f17050dfd7fca036bb244`
- short commit: `4f701cc`
- branch at inspection: `main`
- license found: `LICENSE-MIT`
- ERLoader status: **vendored into `vendor/modengine2/`**

### Why it is included

Mod Engine 2 is the chosen runtime foundation for ERLoader because it already provides:
- Elden Ring support
- TOML-based configuration direction
- launcher-based game startup
- multi-mod file redirection support
- an unfinished but useful WPF frontend precedent

### Notable areas for ERLoader

- `launcher/` — current launch path
- `frontend/` — WPF precedent and information architecture reference
- `src/modengine/` — runtime core
- `src/gametypes/` — game-specific support
- `include/modengine/` — public/native boundaries worth reviewing
- `third-party/` — dependencies that may affect build strategy

### Important caution

The upstream README states development on Mod Engine 2 is discontinued and future work moved elsewhere. ERLoader should therefore treat this import as a fork baseline under its own control.

---

## 2. Elden Ring Mod Loader

- upstream repository: `https://github.com/techiew/EldenRingModLoader`
- inspected local path: `/tmp/EldenRingModLoader`
- inspected commit: `d5c05cb4b6f5e18151355fa170b4ce5b85202165`
- short commit: `d5c05cb`
- branch at inspection: `master`
- license found: **none detected in repository root search**
- ERLoader status: **not vendored; documented as behavioral reference only**

### Why it is referenced

Elden Ring Mod Loader captures the DLL-loading behavior that ERLoader wants to preserve for compatibility, including:
- DLL discovery from the mod-loader folder model
- optional explicit load ordering
- delay semantics between ordered load groups
- config override behavior

### Why it is not copied into the repo yet

No explicit license file was detected during repo inspection. Without clear licensing, copying that source into a hosted public repository is not the safe default.

ERLoader can still use the inspected behavior and public documentation as a compatibility target while keeping implementation original in `src/ERLoader.LoaderCompat/`.

---

## 3. Current ERLoader posture

- Vendor actual third-party source only when provenance and licensing are acceptable.
- Keep imported code isolated under `vendor/`.
- Keep ERLoader-owned logic in `src/` and `ui/`.
- Preserve legacy DLL behavior via clean-room compatibility implementation if direct vendoring is not appropriate.
