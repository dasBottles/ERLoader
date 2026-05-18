# ADR 0002: Upstream Import Strategy

## Status
Accepted

## Context

ERLoader needs two different upstream inputs:

1. a runtime/file-mod foundation from Mod Engine 2
2. behavior compatibility guidance from Elden Ring Mod Loader

These two upstreams are not equal from a repository-management standpoint.

- Mod Engine 2 includes a detectable MIT license and is the selected fork baseline.
- Elden Ring Mod Loader did not present an explicit license file during inspection, which creates risk for directly copying source into a public hosted repository.

## Decision

### Mod Engine 2

Vendor the Mod Engine 2 source tree under:
- `vendor/modengine2/`

Treat it as the imported fork baseline from which ERLoader-owned integration will be built.

### Elden Ring Mod Loader

Do **not** vendor the source tree into the hosted repo at this stage.

Instead:
- keep a reference placeholder under `vendor/elden-mod-loader-reference/`
- document inspected upstream provenance and observed behavior
- implement compatibility behavior originally inside `src/ERLoader.LoaderCompat/`

## Rationale

- This preserves a clear legal/provenance boundary.
- It avoids mixing imported third-party code with original ERLoader code.
- It still preserves the product goal of matching DLL-loading semantics closely.
- It lets ERLoader move forward immediately without waiting on licensing clarification.

## Consequences

### Positive
- Safe default for public repo hygiene
- Clear ownership boundaries
- Easier future review of what is imported vs original
- Compatibility work can proceed without source vendoring

### Negative
- Elden Ring Mod Loader behavior must be re-expressed from inspection rather than extended in-place
- Some edge semantics may require additional reverse verification later
- If permission or licensing is clarified later, the import strategy may need revision

## Follow-up actions

- inventory Mod Engine 2 areas to adapt first
- define DLL compatibility schema in `src/ERLoader.Config/`
- create loader compatibility test fixtures from observed behavior
- revisit vendoring decision only if licensing/permission becomes explicit
