using ERLoader.AppModel;

namespace ERLoader.Wpf.Tests;

public sealed class MainWindowViewModelTests
{
    [Fact]
    public void ImportArchive_for_file_mod_adds_a_new_file_mod_entry()
    {
        var viewModel = MainWindowViewModel.CreatePrototype();
        var before = viewModel.FileMods.Count;

        var imported = viewModel.ImportArchive(@"C:\Downloads\seamless-coop-hotfix.zip", ModImportTarget.FileMod);

        Assert.Equal(before + 1, viewModel.FileMods.Count);
        Assert.Equal("seamless-coop-hotfix", imported.Id);
        Assert.Equal("Seamless Coop Hotfix", imported.Name);
        Assert.Equal(@"imports\filemods\seamless-coop-hotfix.zip", imported.Path);
        Assert.Equal("Archive", imported.Scope);
        Assert.Contains("Imported from archive", imported.Notes);
    }

    [Fact]
    public void ImportArchives_for_dll_mods_adds_entries_and_updates_status_message()
    {
        var viewModel = MainWindowViewModel.CreatePrototype();
        var before = viewModel.DllMods.Count;

        var imported = viewModel.ImportArchives(new[]
        {
            @"C:\Downloads\fps_unlocker_patch.7z",
            @"C:\Downloads\camera_tools_plus.rar"
        }, ModImportTarget.DllMod);

        Assert.Equal(2, imported.Count);
        Assert.Equal(before + 2, viewModel.DllMods.Count);
        Assert.Equal("fps-unlocker-patch", imported[0].Id);
        Assert.Equal("camera-tools-plus", imported[1].Id);
        Assert.Equal("Imported archive", imported[0].Status);
        Assert.Contains("Imported 2 archive", viewModel.LastImportMessage);
    }

    [Fact]
    public void ImportArchive_rejects_unsupported_extensions()
    {
        var viewModel = MainWindowViewModel.CreatePrototype();

        var action = () => viewModel.ImportArchive(@"C:\Downloads\readme.txt", ModImportTarget.FileMod);

        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Contains("archive", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ImportArchive_generates_unique_ids_for_duplicate_archive_names()
    {
        var viewModel = MainWindowViewModel.CreatePrototype();

        var first = viewModel.ImportArchive(@"C:\Downloads\randomizer.zip", ModImportTarget.FileMod);
        var second = viewModel.ImportArchive(@"C:\Other\randomizer.zip", ModImportTarget.FileMod);

        Assert.Equal("randomizer-2", first.Id);
        Assert.Equal("randomizer-3", second.Id);
    }

    [Fact]
    public void ImportArchives_is_atomic_when_any_path_is_invalid()
    {
        var viewModel = MainWindowViewModel.CreatePrototype();
        var before = viewModel.FileMods.Count;

        var action = () => viewModel.ImportArchives(new[]
        {
            @"C:\Downloads\quest-log.zip",
            @"C:\Downloads\readme.txt"
        }, ModImportTarget.FileMod);

        Assert.Throws<ArgumentException>(action);
        Assert.Equal(before, viewModel.FileMods.Count);
    }

    [Fact]
    public void ImportArchive_falls_back_to_a_non_empty_id_for_symbol_only_names()
    {
        var viewModel = MainWindowViewModel.CreatePrototype();

        var imported = viewModel.ImportArchive(@"C:\Downloads\!!!.zip", ModImportTarget.FileMod);

        Assert.Equal("imported-archive", imported.Id);
    }
}
