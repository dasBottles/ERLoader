using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ERLoader.AppModel;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace ERLoader.Wpf;

public partial class MainWindow : MetroWindow
{
    private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = MainWindowViewModel.CreatePrototype();
        ProfilesListBox.SelectedIndex = 0;
        LogsListBox.SelectedIndex = 0;
    }

    private void ActivateSelectedProfileClicked(object sender, RoutedEventArgs e)
    {
        if (ProfilesListBox.SelectedItem is ProfileItem profile)
        {
            ViewModel.SetActiveProfile(profile.Id);
        }
    }

    private void ProfilesSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ProfilesListBox.SelectedItem is ProfileItem profile)
        {
            ViewModel.SetActiveProfile(profile.Id);
        }
    }

    private void RunValidationClicked(object sender, RoutedEventArgs e)
    {
        ViewModel.RunPrototypeValidation();
        MessageBox.Show(this,
            $"Validation completed for {ViewModel.ActiveProfileName}.\n{ViewModel.LaunchReadiness}",
            "ERLoader Prototype",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void SafeRecoveryClicked(object sender, RoutedEventArgs e)
    {
        ViewModel.UseSafeRecoveryProfile();
        ProfilesListBox.SelectedValue = "vanilla";
    }

    private void LaunchPrototypeClicked(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(this,
            $"Prototype launch only.\n\nProfile: {ViewModel.ActiveProfileName}\nStatus: {ViewModel.LaunchReadiness}\n\nThe real runtime hookup comes next.",
            "ERLoader Prototype",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void LogsSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LogsListBox.SelectedItem is LogEntry entry)
        {
            ViewModel.SelectLog(entry);
        }
    }

    private void BrowseFileModArchivesClicked(object sender, RoutedEventArgs e)
    {
        ImportArchivesFromPicker(ModImportTarget.FileMod);
    }

    private void BrowseDllModArchivesClicked(object sender, RoutedEventArgs e)
    {
        ImportArchivesFromPicker(ModImportTarget.DllMod);
    }

    private void FileModArchiveDragOver(object sender, DragEventArgs e)
    {
        HandleArchiveDragOver(e);
    }

    private void DllModArchiveDragOver(object sender, DragEventArgs e)
    {
        HandleArchiveDragOver(e);
    }

    private void FileModArchiveDrop(object sender, DragEventArgs e)
    {
        HandleArchiveDrop(e, ModImportTarget.FileMod);
    }

    private void DllModArchiveDrop(object sender, DragEventArgs e)
    {
        HandleArchiveDrop(e, ModImportTarget.DllMod);
    }

    private void ImportArchivesFromPicker(ModImportTarget target)
    {
        var dialog = new OpenFileDialog
        {
            Title = target == ModImportTarget.FileMod ? "Import file-mod archives" : "Import DLL-mod archives",
            Filter = "Archive files (*.zip;*.7z;*.rar)|*.zip;*.7z;*.rar",
            Multiselect = true,
            CheckFileExists = true
        };

        if (dialog.ShowDialog(this) == true)
        {
            ImportArchives(dialog.FileNames, target, target == ModImportTarget.FileMod ? "file mod" : "DLL mod");
        }
    }

    private void HandleArchiveDragOver(DragEventArgs e)
    {
        if (!TryGetArchiveFiles(e.Data, out _))
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            return;
        }

        e.Effects = DragDropEffects.Copy;
        e.Handled = true;
    }

    private void HandleArchiveDrop(DragEventArgs e, ModImportTarget target)
    {
        if (!TryGetArchiveFiles(e.Data, out var archiveFiles))
        {
            MessageBox.Show(this,
                "Drop one or more .zip, .7z, or .rar archives into the import panel.",
                "Unsupported drop",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        ImportArchives(archiveFiles, target, target == ModImportTarget.FileMod ? "file mod" : "DLL mod");
    }

    private void ImportArchives(IReadOnlyList<string> archiveFiles, ModImportTarget target, string label)
    {
        try
        {
            var imported = ViewModel.ImportArchives(archiveFiles, target);
            if (imported.Count == 0)
            {
                return;
            }

            MessageBox.Show(this,
                $"Imported {imported.Count} {label} archive{(imported.Count == 1 ? string.Empty : "s")}.\n\nLatest status:\n{ViewModel.LastImportMessage}",
                "ERLoader Prototype",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (ArgumentException exception)
        {
            MessageBox.Show(this,
                exception.Message,
                "Archive import failed",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }

    private static bool TryGetArchiveFiles(IDataObject dataObject, out IReadOnlyList<string> archiveFiles)
    {
        archiveFiles = Array.Empty<string>();
        if (!dataObject.GetDataPresent(DataFormats.FileDrop))
        {
            return false;
        }

        if (dataObject.GetData(DataFormats.FileDrop) is not string[] droppedFiles)
        {
            return false;
        }

        var validArchives = droppedFiles
            .Where(file => string.Equals(Path.GetExtension(file), ".zip", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(Path.GetExtension(file), ".7z", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(Path.GetExtension(file), ".rar", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        if (validArchives.Length == 0)
        {
            return false;
        }

        archiveFiles = validArchives;
        return true;
    }
}
