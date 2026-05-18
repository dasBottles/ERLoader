# Elden Ring Mod Loader Reference

This directory is reserved for behavior-reference material related to Elden Ring Mod Loader:
- upstream repo: `https://github.com/techiew/EldenRingModLoader`
- inspected commit: `d5c05cb4b6f5e18151355fa170b4ce5b85202165`
- inspected branch: `master`

## Why the source is not vendored yet

During Milestone 1 inspection, no explicit license file was found in the upstream repository checkout.

Because ERLoader is a hosted public repository, the source is **not copied into this repo yet**. Until licensing/permission is clarified, ERLoader should treat Elden Ring Mod Loader as an external behavioral reference rather than vendored source.

## What ERLoader should preserve

The following observed semantics should be preserved as closely as practical in ERLoader's compatibility layer:
- load all DLLs found in the mod-loader folder model
- support explicit load ordering
- support delayed load behavior between ordered DLL groups
- allow config-driven load order overrides
- keep logging clear enough for users to troubleshoot load failures

## Next step

If licensing or permission is clarified later, this directory can be replaced with a properly attributed source import or a narrower set of allowed reference files.
