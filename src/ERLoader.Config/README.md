# ERLoader.Config

This directory is reserved for ERLoader-owned configuration and profile logic.

## Milestone 2 intent

The first goal is to lock the canonical configuration contract before committing to a specific implementation language or serialization library in this repo.

That means Milestone 2 currently provides:
- schema documentation
- valid fixture examples
- invalid fixture examples
- contract tests that keep the examples and rules aligned

## Planned responsibilities

Later implementation in this area should cover:
- TOML parsing and writing
- profile validation
- cross-reference validation
- import mapping from legacy formats
- backup-before-save behavior
- launcher-facing model objects

## Current contract reference

See:
- `docs/architecture/config-schema.md`
- `tests/config/test_profile_schema.py`
- `tests/config/fixtures/`
