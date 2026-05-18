# ERLoader Repository Bootstrap Plan

> **For Hermes:** Use subagent-driven-development skill to implement this plan task-by-task.

**Goal:** Turn the currently empty `ERLoader` repository into the concrete starting point for the unified Elden Ring launcher described in the implementation brief.

**Architecture:** Because this repo currently contains only `LICENSE`, the first milestone should be a bootstrap milestone: establish repository structure, import the Mod Engine 2 codebase into a dedicated backend area, preserve a clean place for Elden Mod Loader compatibility work, and add a WPF shell for the future GUI. Do not try to solve import, runtime DLL orchestration, and polished UX in the first commit.

**Tech Stack:** Git, C++ backend, WPF + MahApps.Metro frontend, TOML config, Markdown docs.

---

## 1. Current Repo Reality

As of inspection, `https://github.com/dasBottles/ERLoader` currently has:

- branch: `main`
- commit history: one initial commit
- tracked files: `LICENSE` only

That means there is no existing app structure to extend yet. The next correct step is **bootstrap planning against an empty repo**, not file-by-file modification of an existing codebase.

---

## 2. Recommended Initial Repo Layout

Create this structure first:

```text
ERLoader/
├── LICENSE
├── README.md
├── .gitignore
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

### Why this shape
- `vendor/modengine2/` keeps the imported upstream backend isolated.
- `vendor/elden-mod-loader-reference/` keeps legacy behavior reference code available without pretending it is the shipping runtime.
- `src/ERLoader.LoaderCompat/` is where the compatibility layer for DLL semantics should live.
- `ui/ERLoader.Wpf/` avoids mixing the user-facing desktop app with backend/runtime code too early.
- `docs/decisions/` gives you a place to lock architecture choices as they change.

---

## 3. How the Existing Brief Maps to This Repo

### A. Fork-and-extend Mod Engine 2
Map to:
- `vendor/modengine2/` for the imported upstream baseline
- `src/ERLoader.Backend/` for ERLoader-owned adaptation layer

### B. Preserve Elden Mod Loader semantics
Map to:
- `vendor/elden-mod-loader-reference/` for reference behavior only
- `src/ERLoader.LoaderCompat/` for actual compatibility implementation

### C. Canonical TOML model
Map to:
- `src/ERLoader.Config/`
- `tests/config/`
- `tests/import/`

### D. Sleek Windows GUI
Map to:
- `ui/ERLoader.Wpf/`
- later visual assets under `ui/ERLoader.Wpf/Assets/`

---

## 4. Recommended Milestones

## Milestone 0 — Repository bootstrap

**Objective:** Create the structure and documentation needed to start cleanly.

**Deliverables:**
- root `README.md`
- root `.gitignore`
- `docs/architecture/overview.md`
- `docs/decisions/0001-repo-layout.md`
- this plan committed into `docs/plans/`
- empty scaffolding directories under `src/`, `ui/`, `tests/`, `vendor/`

**Success criteria:**
- a new contributor can clone the repo and understand the intended layout
- the repo structure matches the product architecture

## Milestone 1 — Upstream code import baseline

**Objective:** Bring in the two upstream code references cleanly.

**Deliverables:**
- import Mod Engine 2 source into `vendor/modengine2/`
- import Elden Mod Loader reference source into `vendor/elden-mod-loader-reference/`
- add `docs/architecture/upstream-inventory.md`
- add `docs/decisions/0002-upstream-import-strategy.md`

**Success criteria:**
- both upstream codebases are present and documented
- ERLoader-owned code is still clearly separated from imported code

## Milestone 2 — Config domain and profile model

**Objective:** Make the TOML schema real before building runtime orchestration.

**Deliverables:**
- schema doc in `docs/architecture/config-schema.md`
- parser/writer scaffolding in `src/ERLoader.Config/`
- sample config fixtures in `tests/config/fixtures/`
- profile validation tests in `tests/config/`

**Success criteria:**
- a profile can represent file mods, DLL mods, load order, delays, settings, and metadata
- invalid configs fail predictably

## Milestone 3 — Launcher shell and navigation

**Objective:** Get the desktop application shell running early.

**Deliverables:**
- WPF app scaffold in `ui/ERLoader.Wpf/`
- MahApps.Metro setup
- initial screens: Dashboard, Profiles, File Mods, DLL Mods, Validation, Logs, Settings
- mock data only at first

**Success criteria:**
- the app launches on Windows
- the main navigation and information hierarchy are validated before backend integration

## Milestone 4 — Runtime integration path

**Objective:** Connect launcher state to the forked backend.

**Deliverables:**
- backend adapter in `src/ERLoader.Backend/`
- launch preparation pipeline
- generated runtime config output
- structured launch results/logs

**Success criteria:**
- one launcher path prepares a profile and launches through the unified runtime

## Milestone 5 — DLL compatibility layer

**Objective:** Add Elden Mod Loader–style behavior without reintroducing a second launcher.

**Deliverables:**
- compatibility model in `src/ERLoader.LoaderCompat/`
- support for `dllmods/`
- load-order and delay handling
- logging of DLL success/failure states

**Success criteria:**
- DLL mods are represented in one profile model and handled in one launch flow

## Milestone 6 — Import, validation, and safety

**Objective:** Make the app practical for real mod users.

**Deliverables:**
- importers for legacy setups
- conflict detection
- safe/vanilla recovery profile
- backup-before-save behavior
- basic logs view

**Success criteria:**
- a user can import, validate, understand blockers, and recover safely

---

## 5. First Concrete Files I Recommend Creating

If I start work in this repo, these should be the first repo-owned files:

- `README.md`
- `.gitignore`
- `docs/architecture/overview.md`
- `docs/decisions/0001-repo-layout.md`
- `docs/plans/2026-05-17-erloader-repo-bootstrap-plan.md`
- `src/ERLoader.Backend/.gitkeep`
- `src/ERLoader.LoaderCompat/.gitkeep`
- `src/ERLoader.Config/.gitkeep`
- `src/ERLoader.Launcher/.gitkeep`
- `ui/ERLoader.Wpf/.gitkeep`
- `tests/config/.gitkeep`
- `tests/import/.gitkeep`
- `tests/validation/.gitkeep`
- `vendor/modengine2/.gitkeep`
- `vendor/elden-mod-loader-reference/.gitkeep`

---

## 6. Key Architectural Advice for This Repo

1. **Do not start by copying code directly into the repo root.**
   Keep third-party code quarantined under `vendor/`.

2. **Do not mix compatibility code into the imported upstream tree on day one.**
   First establish the clean seam between imported code and ERLoader-owned code.

3. **Do not begin with the GUI polish pass.**
   Start with repo structure, imports, and config model so the UI has a stable contract.

4. **Do not create separate file-mod and DLL-mod profile systems.**
   The TOML model should be unified from the start.

5. **Do not treat the reference Elden Mod Loader source as the shipping architecture.**
   Use it to preserve behavior, not to preserve a second product shape.

---

## 7. Best Next Implementation Move

Because the repo is effectively blank, the highest-leverage next action is:

**Bootstrap the repository skeleton and commit the architecture/docs baseline first.**

That gives us a safe place to then:
1. import Mod Engine 2
2. import Elden Mod Loader reference code
3. wire the config model
4. build the WPF shell

---

## 8. What I Can Do Next In This Repo

I can now proceed with either of these paths:

### Path A — bootstrap the repo immediately
I create the folders, README, `.gitignore`, architecture docs, and decision records in this repo.

### Path B — write the full phase-1 execution checklist
I turn Milestone 0 and Milestone 1 into a more granular task list with exact files and commit boundaries.

For this repo, **Path A is the strongest next step** because there is no existing scaffold to adapt.
