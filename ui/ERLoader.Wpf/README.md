# ERLoader.Wpf Prototype

This is the first clickable Windows prototype for ERLoader.

## What it is

A WPF + MahApps.Metro shell that demonstrates the intended launcher flow for:
- profile selection
- file mod visibility
- DLL mod visibility
- launch readiness summaries
- validation blockers vs warnings
- logs browsing
- safe/vanilla recovery switching

## What it is not yet

This prototype is **not** wired into:
- a live Elden Ring install
- real Mod Engine 2 launch execution
- real TOML read/write
- importers from legacy setups
- real conflict scanning

All data is currently mock/sample data meant for product and UX testing.

## Open in Visual Studio

- Recommended: Visual Studio 2022 on Windows
- Project: `ERLoader.Wpf.csproj`
- Solution: `..\..\ERLoader.sln`
- Target framework: `net8.0-windows`

## Expected test flow

1. Launch the app.
2. Switch between profiles in the left rail.
3. Review how dashboard, validation, and counts change.
4. Open the File Mods and DLL Mods tabs.
5. Test the Safe Recovery button.
6. Trigger the prototype launch and validation buttons.
7. Use that experience to judge layout, information density, and workflow fit.

## Prototype feedback to collect

When testing, the most useful feedback will be:
- does the information hierarchy feel right?
- are the tabs/screens the right ones?
- is the safe recovery flow obvious enough?
- do file mods and DLL mods feel unified without becoming confusing?
- what feels too dense, too sparse, or too technical?

## Current implementation files

- `App.xaml`
- `MainWindow.xaml`
- `MainWindow.xaml.cs`
- `MainWindowViewModel.cs`
- `ERLoader.Wpf.csproj`

## Next likely implementation step

After UX approval, the next layer should be:
1. real profile/config IO
2. import/migration from legacy setups
3. validation logic backed by actual rules
4. runtime launch integration with the Mod Engine 2 baseline
