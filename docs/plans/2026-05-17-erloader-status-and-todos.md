# ERLoader Status + To-Dos

> Snapshot taken after prototype commit `9460327` (`feat: add archive import interactions`).

## Current status

ERLoader now has a downloadable WPF prototype that combines the current launcher shell, profile switching UX, validation/log panels, and prototype import flows for both file mods and DLL mods. The project is still in the prototype stage: the GUI and app-model behavior are implemented, but real persistence, extraction, migration, and launch/runtime integration are not yet wired up.

## Completed work

### 1. Repository bootstrap
- Created the base repository structure under:
  - `docs/`
  - `vendor/`
  - `src/`
  - `ui/`
  - `tests/`
  - `build/`
- Added root project documentation and initial ADRs.
- Established the repo ownership split between ERLoader code and imported upstream code.

### 2. Upstream intake and provenance
- Vendored **Mod Engine 2** under `vendor/modengine2/`.
- Documented upstream inventory and import strategy.
- Chose to treat **Elden Ring Mod Loader** as a behavioral reference only because no explicit license file was found during inspection.

### 3. Product and architecture decisions locked in
- Elden Ring only.
- Unified launcher and GUI.
- Unified TOML config model.
- Dedicated `dllmods/` concept.
- WPF + MahApps.Metro for the desktop UI.
- Portable ZIP distribution for v1.
- Safe recovery / vanilla profile support.
- Launch should be blocked on missing game path, invalid config, or hard conflicts.

### 4. Config contract foundation
- Defined the canonical TOML schema in:
  - `docs/architecture/config-schema.md`
- Added valid and invalid config fixtures.
- Added Python contract tests for the schema examples.

### 5. Initial WPF prototype
- Added a Windows desktop prototype in:
  - `ui/ERLoader.Wpf/`
- Added the root solution:
  - `ERLoader.sln`
- Implemented the core shell experience:
  - profile list
  - overview panel
  - file mods panel
  - DLL mods panel
  - validation panel
  - logs panel
  - safe recovery action
  - simulated launch action

### 6. Shared prototype app-model layer
- Extracted non-WPF behavior into:
  - `src/ERLoader.AppModel/`
- This now holds shared prototype state and logic for:
  - profiles
  - validation items
  - logs
  - import handling
  - ID/slug generation
- Added .NET unit tests in:
  - `tests/ERLoader.Wpf.Tests/`

### 7. UX polish + archive import iteration
- Fixed the profile selection/highlight rendering problem in the WPF prototype.
- Added archive import interactions for **File Mods**:
  - browse button
  - drag/drop zone
- Added archive import interactions for **DLL Mods**:
  - browse button
  - drag/drop zone
- Added prototype handling for supported archives:
  - `.zip`
  - `.7z`
  - `.rar`
- Improved duplicate-name handling and fallback slug generation for imported items.

### 8. Build, packaging, and downloadable artifacts
- Installed/fixed local .NET 8 tooling in this environment.
- Built and published Windows prototype outputs.
- Packaged downloadable artifacts under:
  - `artifacts/ERLoader-Wpf-win-x64-self-contained.zip`
  - `artifacts/ERLoader-Wpf-win-x64-framework-dependent.zip`
  - `artifacts/SHA256SUMS.txt`

## Latest validated state

### Git
- Branch: `main`
- Latest commit: `9460327` — `feat: add archive import interactions`

### Automated verification
- `.NET unit tests`
  - `dotnet test tests/ERLoader.Wpf.Tests/ERLoader.Wpf.Tests.csproj -v minimal`
  - Result: `6 passed`
- `.NET build`
  - `dotnet build ERLoader.sln -c Release -p:EnableWindowsTargeting=true`
  - Result: success
- `Python config contract tests`
  - `pytest tests/ -q`
  - Result: `6 passed`

### Current prototype artifacts
- `ERLoader-Wpf-win-x64-self-contained.zip`
  - SHA256: `8ba10e1a610611b40aa580010c8843d656b6f69f89bf38912e2c9e743d4aa112`
- `ERLoader-Wpf-win-x64-framework-dependent.zip`
  - SHA256: `285630f89b9b0e2c46ac68bcc4cb34def141f6964fee68cfefd8678c3c7cc5d0`

## Open To-Dos

### Priority 1 — make the prototype real
- [ ] Implement real TOML load/save using the canonical schema.
- [ ] Persist profiles, mod entries, settings, and safe profile state.
- [ ] Add game path selection and persistence.
- [ ] Replace mock validation data with validation derived from persisted config/model state.

### Priority 2 — make import actually install mods
- [ ] Replace metadata-only archive staging with real extraction/import behavior.
- [ ] Define install layout rules for file mods vs DLL mods.
- [ ] Add archive inspection to identify likely mod type before import.
- [ ] Add overwrite/conflict prompts for duplicate installs.
- [ ] Add remove/reimport flows for imported mods.

### Priority 3 — compatibility and migration
- [ ] Add import/migration from existing Mod Engine 2 setups.
- [ ] Add import/migration from Elden Mod Loader-style DLL setups.
- [ ] Preserve expected DLL load-order / delay semantics in the real runtime path.
- [ ] Add compatibility notes for mixed mod environments.

### Priority 4 — runtime integration
- [ ] Wire the launcher to real Mod Engine 2 runtime behavior.
- [ ] Decide where ERLoader-owned launch code ends and vendored runtime modifications begin.
- [ ] Verify native build/toolchain needs for Mod Engine 2 integration (`cmake` is still missing in this environment).
- [ ] Replace the simulated launch action with a real launch pipeline.

### Priority 5 — user safety and diagnostics
- [ ] Implement blocker-level validation for missing game path, bad config parse, and hard conflicts.
- [ ] Add a clearer recovery/vanilla launch path.
- [ ] Improve logs from prototype text entries into actionable runtime diagnostics.
- [ ] Add better error surfaces around failed imports and invalid archives.

### Priority 6 — UX follow-up
- [ ] Gather tester feedback on the revised profile highlighting.
- [ ] Gather tester feedback on browse/drag-drop archive import UX.
- [ ] Add progress/status feedback for large imports.
- [ ] Add empty-state polish and contextual help text.
- [ ] Decide whether import should remain staged-first or become immediate install by default.

### Priority 7 — delivery hygiene
- [ ] Move large downloadable artifacts to GitHub Releases instead of keeping large ZIPs in-repo.
- [ ] Add release/version metadata to the prototype.
- [ ] Add a lightweight changelog or milestone tracker.
- [ ] Add CI for build/test verification.

## Suggested next checkpoint

When work resumes, the highest-value next slice is:

1. real TOML persistence
2. real archive extraction/import
3. real validation based on saved state
4. real launch pipeline hookup

That sequence keeps the current GUI prototype while steadily converting it into a working mod manager.
