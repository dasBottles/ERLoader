from __future__ import annotations

from pathlib import Path
import tomllib

import pytest

FIXTURES_ROOT = Path(__file__).parent / "fixtures"
VALID_FIXTURES = [
    "valid/minimal-profiles.toml",
    "valid/full-profiles.toml",
]
INVALID_FIXTURES = [
    "invalid/missing-safe-profile.toml",
    "invalid/duplicate-profile-id.toml",
    "invalid/unknown-file-mod-reference.toml",
    "invalid/negative-dll-delay.toml",
]


def load_toml(relative_path: str) -> dict:
    fixture_path = FIXTURES_ROOT / relative_path
    with fixture_path.open("rb") as handle:
        return tomllib.load(handle)


def validate_profiles_document(data: dict) -> None:
    app = data["app"]
    defaults = data["defaults"]
    profiles = data["profiles"]
    file_mods = data.get("file_mods", [])
    dll_mods = data.get("dll_mods", [])

    assert app["version"] == 1
    assert isinstance(app["active_profile"], str) and app["active_profile"]
    assert isinstance(defaults["safe_profile"], str) and defaults["safe_profile"]
    assert isinstance(profiles, list) and profiles

    profile_ids = [profile["id"] for profile in profiles]
    assert len(profile_ids) == len(set(profile_ids)), "profile IDs must be unique"
    assert defaults["safe_profile"] in profile_ids, "safe_profile must reference an existing profile"
    assert app["active_profile"] in profile_ids, "active_profile must reference an existing profile"

    file_mod_ids = {entry["id"] for entry in file_mods}
    dll_mod_ids = {entry["id"] for entry in dll_mods}

    for entry in dll_mods:
        assert entry["load_delay_ms"] >= 0, "DLL load delay must not be negative"
        assert entry["load_order"] >= 0, "DLL load order must not be negative"

    for profile in profiles:
        for mod_id in profile.get("file_mod_ids", []):
            assert mod_id in file_mod_ids, f"unknown file mod reference: {mod_id}"
        for mod_id in profile.get("dll_mod_ids", []):
            assert mod_id in dll_mod_ids, f"unknown DLL mod reference: {mod_id}"


@pytest.mark.parametrize("relative_path", VALID_FIXTURES)
def test_valid_profile_documents_match_contract(relative_path: str) -> None:
    document = load_toml(relative_path)
    validate_profiles_document(document)


@pytest.mark.parametrize("relative_path", INVALID_FIXTURES)
def test_invalid_profile_documents_break_contract(relative_path: str) -> None:
    document = load_toml(relative_path)
    with pytest.raises(AssertionError):
        validate_profiles_document(document)
