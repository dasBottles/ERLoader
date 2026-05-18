# ERLoader Config Foundation Plan

> **For Hermes:** Use subagent-driven-development skill to implement this plan task-by-task.

**Goal:** Establish a stable, test-backed canonical TOML contract for ERLoader profiles before deeper launcher or runtime integration begins.

**Architecture:** Treat the config layer as the contract between the future WPF launcher, import/migration logic, and the Mod Engine 2–based runtime path. In the current repo state, prioritize schema documentation and contract fixtures first, then introduce implementation code only after those fixtures lock the expected behavior.

**Tech Stack:** TOML, Markdown docs, Python contract tests for fixture verification during bootstrap, future C++/launcher integration.

---

## Task 1: Lock the schema contract

**Objective:** Define the canonical v1 TOML structure in a way backend and UI work can share.

**Files:**
- Create: `docs/architecture/config-schema.md`
- Reference: `docs/architecture/upstream-inventory.md`

**Verification:**
- Schema doc includes root sections, required fields, cross-reference rules, and at least one minimal + one full example.

## Task 2: Add contract fixtures

**Objective:** Add representative valid and invalid examples so the schema is executable, not just descriptive.

**Files:**
- Create: `tests/config/fixtures/valid/minimal-profiles.toml`
- Create: `tests/config/fixtures/valid/full-profiles.toml`
- Create: `tests/config/fixtures/invalid/*.toml`

**Verification:**
- Valid fixtures represent intended launcher behavior.
- Invalid fixtures isolate one rule break each.

## Task 3: Add bootstrap contract tests

**Objective:** Ensure fixture examples continue to match the current schema contract.

**Files:**
- Create: `tests/config/test_profile_schema.py`

**Step 1: Write failing test**
- Add tests that expect valid and invalid fixture files to exist and satisfy schema rules.

**Step 2: Run test to verify failure**
- Run: `pytest tests/config/test_profile_schema.py::test_valid_profile_documents_match_contract -v`
- Expected: FAIL because fixture files do not exist yet.

**Step 3: Add the minimal implementation assets**
- Create the fixture files and contract rules needed for the bootstrap phase.

**Step 4: Run test to verify pass**
- Run: `pytest tests/config/test_profile_schema.py -v`
- Expected: PASS

**Step 5: Run all tests**
- Run: `pytest tests/ -q`
- Expected: PASS

## Task 4: Prepare the implementation seam

**Objective:** Reserve a clear home for future repo-owned config implementation.

**Files:**
- Create: `src/ERLoader.Config/README.md`

**Verification:**
- The directory clearly states its future responsibilities and points at the schema/fixtures.

## Task 5: Next implementation slice

**Objective:** Prepare for actual parser/validator code after the contract is stable.

**Likely next files:**
- `src/ERLoader.Config/` implementation files once toolchain choice is committed
- `tests/import/` fixtures for Mod Engine 2 and Elden Mod Loader migration cases
- `tests/validation/` cases for blocker vs warning behavior

**Verification:**
- Future work can point to the existing schema contract rather than redesigning config shape mid-stream.
