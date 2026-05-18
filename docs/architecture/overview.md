# ERLoader Architecture Overview

## Goal

Build a Windows-first Elden Ring mod manager and launcher that unifies Mod Engine 2 file-based mods and Elden Mod Loader–style DLL mods behind one launcher and one TOML configuration model.

## Core product decisions

- Target game: Elden Ring only
- Backend strategy: fork and extend Mod Engine 2 directly
- DLL compatibility goal: preserve Elden Mod Loader semantics as closely as practical
- Canonical config format: TOML
- DLL folder strategy: `dllmods/`
- UI stack: WPF + MahApps.Metro
- Distribution model: portable zip

## Top-level architecture

### 1. Imported upstream baselines

- `vendor/modengine2/`
- `vendor/elden-mod-loader-reference/`

These directories exist to preserve a clear boundary between imported third-party code and ERLoader-owned code.

### 2. ERLoader-owned backend

`src/ERLoader.Backend/`

This layer will adapt the imported Mod Engine 2 baseline into ERLoader's canonical launch flow.

Responsibilities:
- launch preparation
- runtime argument/config generation
- error/result collection
- integration seam for DLL compatibility logic

### 3. DLL compatibility layer

`src/ERLoader.LoaderCompat/`

Responsibilities:
- represent DLL mod entries
- map profile state to load order and delay behavior
- preserve legacy loading expectations as closely as practical
- report DLL load success/failure to logs and UI

### 4. Config domain

`src/ERLoader.Config/`

Responsibilities:
- canonical TOML schema
- profile load/write
- validation
- import mapping from legacy formats
- backups before rewrite

### 5. WPF launcher UI

`ui/ERLoader.Wpf/`

Responsibilities:
- dashboard
- profile management
- file mod management
- DLL mod management
- validation/conflict presentation
- logs
- settings

## Launch ownership

The unified launcher must own:
- game path detection and validation
- profile resolution
- config read/write
- pre-launch validation
- runtime preparation
- unified launch execution

There should not be a separate standalone launcher flow for DLL mods.

## Safety expectations

- block launch on missing game path
- block launch on invalid config parse
- block launch on hard conflicts
- keep safe/vanilla recovery mode available
- back up rewritten config before save
