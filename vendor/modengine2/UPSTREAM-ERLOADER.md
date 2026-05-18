# Upstream Provenance for ERLoader

This directory contains a vendored snapshot of Mod Engine 2 used as the baseline runtime foundation for ERLoader.

## Provenance

- upstream repository: `https://github.com/soulsmods/ModEngine2`
- imported from inspected local clone: `/tmp/ModEngine2`
- upstream commit: `4f701ccd0e2f6ddb106f17050dfd7fca036bb244`
- upstream branch at import: `main`
- detected license file: `LICENSE-MIT`

## ERLoader policy

- Treat this directory as imported third-party baseline code.
- Keep ERLoader-owned adaptation logic outside this directory where practical.
- Record major import/rebase events in `docs/architecture/upstream-inventory.md`.
- Prefer additive integration layers before making invasive upstream edits.
