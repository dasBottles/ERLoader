# ERLoader Config Schema

## Purpose

This document defines the initial canonical TOML contract for ERLoader's launcher-owned configuration.

For Milestone 2, the goal is not to finalize every future field. The goal is to lock the v1 data model strongly enough that backend work, import work, and UI work can all target the same structure.

## Design principles

- One canonical launcher-owned TOML document for app state and profile selection
- File mods and DLL mods are first-class top-level collections
- Profiles reference mod IDs instead of duplicating full mod objects
- A safe recovery profile is explicit and must always exist
- The config stays hand-editable, but the app may normalize formatting on save
- Launch-blocking vs warning-only conditions should be derivable from config plus validation rules

## Root sections

A `profiles.toml` document currently uses these root sections:

- `[app]`
- `[game]`
- `[defaults]`
- `[[file_mods]]`
- `[[dll_mods]]`
- `[[profiles]]`

## Root table definitions

### `[app]`

| Field | Type | Required | Notes |
|---|---|---:|---|
| `version` | integer | yes | Start with `1` for v1 schema generation |
| `active_profile` | string | yes | Must reference an existing `profiles[].id` |

### `[game]`

| Field | Type | Required | Notes |
|---|---|---:|---|
| `type` | string | yes | For v1 this should be `"eldenring"` |
| `game_path` | string | yes | Expected executable path |

### `[defaults]`

| Field | Type | Required | Notes |
|---|---|---:|---|
| `hide_advanced` | bool | yes | UI default |
| `show_logs` | bool | yes | UI default |
| `safe_profile` | string | yes | Must reference an existing `profiles[].id` |

## Collection definitions

### `[[file_mods]]`

Represents a file-based mod root handled through the Mod Engine 2 path model.

| Field | Type | Required | Notes |
|---|---|---:|---|
| `id` | string | yes | Unique stable identifier |
| `name` | string | yes | UI-facing label |
| `path` | string | yes | Relative path under managed mod roots |
| `enabled` | bool | yes | Global availability toggle |
| `kind` | string | yes | Use `"root"` in v1 |

### `[[dll_mods]]`

Represents a DLL mod managed through the compatibility layer.

| Field | Type | Required | Notes |
|---|---|---:|---|
| `id` | string | yes | Unique stable identifier |
| `name` | string | yes | UI-facing label |
| `path` | string | yes | Expected DLL path, usually under `dllmods/` |
| `enabled` | bool | yes | Global availability toggle |
| `required` | bool | yes | Missing required DLLs should block launch |
| `load_order` | integer | yes | Must be zero or greater |
| `load_delay_ms` | integer | yes | Must be zero or greater |

### `[[profiles]]`

Represents a launchable user-facing profile.

| Field | Type | Required | Notes |
|---|---|---:|---|
| `id` | string | yes | Unique stable identifier |
| `name` | string | yes | User-visible profile name |
| `description` | string | yes | Friendly explanation |
| `file_mod_ids` | array[string] | yes | Every ID must exist in `file_mods` |
| `dll_mod_ids` | array[string] | yes | Every ID must exist in `dll_mods` |
| `allow_warnings` | bool | yes | Whether warnings can be tolerated at launch |
| `last_used` | string (ISO 8601) | no | Optional timestamp for launcher UX |

## Cross-reference rules

The Milestone 2 contract currently requires:

1. `app.version == 1`
2. `app.active_profile` references an existing profile ID
3. `defaults.safe_profile` references an existing profile ID
4. profile IDs are unique
5. file mod IDs are unique
6. DLL mod IDs are unique
7. every `profiles[].file_mod_ids[]` entry references an existing `file_mods[].id`
8. every `profiles[].dll_mod_ids[]` entry references an existing `dll_mods[].id`
9. every `dll_mods[].load_order` is zero or greater
10. every `dll_mods[].load_delay_ms` is zero or greater

## Example minimal document

```toml
[app]
version = 1
active_profile = "vanilla"

[game]
type = "eldenring"
game_path = "C:\\Steam\\steamapps\\common\\ELDEN RING\\Game\\eldenring.exe"

[defaults]
hide_advanced = true
show_logs = false
safe_profile = "vanilla"

[[profiles]]
id = "vanilla"
name = "Vanilla Recovery"
description = "Safe recovery profile"
file_mod_ids = []
dll_mod_ids = []
allow_warnings = false
```

## Example fuller document

```toml
[app]
version = 1
active_profile = "casual-coop"

[game]
type = "eldenring"
game_path = "C:\\Steam\\steamapps\\common\\ELDEN RING\\Game\\eldenring.exe"

[defaults]
hide_advanced = true
show_logs = false
safe_profile = "vanilla"

[[file_mods]]
id = "seamless-coop"
name = "Seamless Co-op"
path = "mods\\seamless-coop"
enabled = true
kind = "root"

[[file_mods]]
id = "randomizer"
name = "Randomizer"
path = "mods\\randomizer"
enabled = false
kind = "root"

[[dll_mods]]
id = "camera-tools"
name = "CameraTools.dll"
path = "dllmods\\CameraTools.dll"
enabled = true
required = true
load_order = 1
load_delay_ms = 1000

[[profiles]]
id = "casual-coop"
name = "Casual Co-op"
description = "Regular co-op play"
file_mod_ids = ["seamless-coop"]
dll_mod_ids = ["camera-tools"]
allow_warnings = false
last_used = "2026-05-17T19:00:00Z"

[[profiles]]
id = "vanilla"
name = "Vanilla Recovery"
description = "Safe recovery profile"
file_mod_ids = []
dll_mod_ids = []
allow_warnings = false
```

## Milestone 2 scope boundary

This schema document intentionally does not yet cover:
- importer metadata
- conflict records written back into config
- per-profile launch argument overrides
- richer DLL grouping semantics
- provenance/manifests/versioning for mods

Those can be added later without blocking the config foundation work already underway.
