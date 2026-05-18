# ERLoader Prototype Artifacts

These are packaged Windows builds of the current ERLoader WPF prototype.

## Files

- `ERLoader-Wpf-win-x64-self-contained.zip`
  - Recommended for testers
  - Includes the required .NET runtime
  - Extract and run `ERLoader.Wpf.exe`

- `ERLoader-Wpf-win-x64-framework-dependent.zip`
  - Smaller package
  - Requires the .NET 8 Desktop Runtime to already be installed on Windows

- `SHA256SUMS.txt`
  - Integrity hashes for both zip files

## Notes

This prototype is currently a functional GUI shell for UX and flow testing.
It is not yet wired into the real Mod Engine 2 launch/runtime path.
