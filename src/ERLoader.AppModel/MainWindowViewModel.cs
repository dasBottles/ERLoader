using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace ERLoader.AppModel;

public sealed class MainWindowViewModel : INotifyPropertyChanged
{
    private static readonly HashSet<string> SupportedArchiveExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".zip",
        ".7z",
        ".rar"
    };

    private string _activeProfileId = string.Empty;
    private string _activeProfileName = string.Empty;
    private string _launchReadiness = string.Empty;
    private string _headlineStatus = string.Empty;
    private string _selectedLogText = string.Empty;
    private string _lastImportMessage = "No archives imported yet.";
    private int _blockerCount;
    private int _warningCount;

    public ObservableCollection<ProfileItem> Profiles { get; } = new();
    public ObservableCollection<FileModItem> FileMods { get; } = new();
    public ObservableCollection<DllModItem> DllMods { get; } = new();
    public ObservableCollection<ValidationItem> ValidationItems { get; } = new();
    public ObservableCollection<LogEntry> Logs { get; } = new();

    public string ActiveProfileId
    {
        get => _activeProfileId;
        private set => SetField(ref _activeProfileId, value);
    }

    public string ActiveProfileName
    {
        get => _activeProfileName;
        private set => SetField(ref _activeProfileName, value);
    }

    public string LaunchReadiness
    {
        get => _launchReadiness;
        private set => SetField(ref _launchReadiness, value);
    }

    public string HeadlineStatus
    {
        get => _headlineStatus;
        private set => SetField(ref _headlineStatus, value);
    }

    public int BlockerCount
    {
        get => _blockerCount;
        private set => SetField(ref _blockerCount, value);
    }

    public int WarningCount
    {
        get => _warningCount;
        private set => SetField(ref _warningCount, value);
    }

    public string SelectedLogText
    {
        get => _selectedLogText;
        set => SetField(ref _selectedLogText, value);
    }

    public string LastImportMessage
    {
        get => _lastImportMessage;
        private set => SetField(ref _lastImportMessage, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public static MainWindowViewModel CreatePrototype()
    {
        var viewModel = new MainWindowViewModel();

        viewModel.Profiles.Add(new ProfileItem
        {
            Id = "casual-coop",
            Name = "Casual Co-op",
            Description = "Regular co-op stack with lightweight helpers.",
            IsSafe = false,
            ReadyLabel = "Ready with warnings"
        });

        viewModel.Profiles.Add(new ProfileItem
        {
            Id = "randomizer-lab",
            Name = "Randomizer Lab",
            Description = "Experimental stack with multiple overlapping gameplay edits.",
            IsSafe = false,
            ReadyLabel = "Blocked"
        });

        viewModel.Profiles.Add(new ProfileItem
        {
            Id = "vanilla",
            Name = "Vanilla Recovery",
            Description = "Safe fallback profile with no file or DLL mods enabled.",
            IsSafe = true,
            ReadyLabel = "Ready"
        });

        viewModel.FileMods.Add(new FileModItem
        {
            Id = "seamless-coop",
            Name = "Seamless Co-op",
            Path = @"mods\seamless-coop",
            Enabled = true,
            Scope = "Root",
            Notes = "Healthy"
        });

        viewModel.FileMods.Add(new FileModItem
        {
            Id = "randomizer",
            Name = "Randomizer",
            Path = @"mods\randomizer",
            Enabled = true,
            Scope = "Root",
            Notes = "Regulation overlap with Reforged preset"
        });

        viewModel.FileMods.Add(new FileModItem
        {
            Id = "reforged-ui",
            Name = "Reforged UI Pack",
            Path = @"mods\reforged-ui",
            Enabled = false,
            Scope = "Root",
            Notes = "Disabled by default"
        });

        viewModel.DllMods.Add(new DllModItem
        {
            Id = "camera-tools",
            Name = "CameraTools.dll",
            Path = @"dllmods\CameraTools.dll",
            Enabled = true,
            Required = true,
            LoadOrder = 1,
            LoadDelayMs = 1000,
            Status = "Ready"
        });

        viewModel.DllMods.Add(new DllModItem
        {
            Id = "discord-presence",
            Name = "DiscordPresence.dll",
            Path = @"dllmods\DiscordPresence.dll",
            Enabled = false,
            Required = false,
            LoadOrder = 2,
            LoadDelayMs = 1000,
            Status = "Optional"
        });

        viewModel.DllMods.Add(new DllModItem
        {
            Id = "fps-unlocker",
            Name = "FPSUnlocker.dll",
            Path = @"dllmods\FPSUnlocker.dll",
            Enabled = true,
            Required = false,
            LoadOrder = 3,
            LoadDelayMs = 1500,
            Status = "Delayed group"
        });

        viewModel.Logs.Add(new LogEntry
        {
            Title = "Launcher Session",
            Category = "launcher.log",
            Content = "[22:41:03] Loaded profiles.toml\n[22:41:03] Active profile: casual-coop\n[22:41:04] Validation completed with 1 warning."
        });

        viewModel.Logs.Add(new LogEntry
        {
            Title = "Validation Report",
            Category = "validation.log",
            Content = "Warning: Optional DiscordPresence.dll disabled.\nHint: Enable if you want rich presence in co-op sessions."
        });

        viewModel.Logs.Add(new LogEntry
        {
            Title = "Last Launch Preview",
            Category = "launch-preview.log",
            Content = "Would launch Elden Ring via the unified Mod Engine 2 path with dllmods CameraTools.dll and FPSUnlocker.dll queued."
        });

        viewModel.SetActiveProfile("casual-coop");
        viewModel.SelectedLogText = viewModel.Logs[0].Content;
        return viewModel;
    }

    public void SetActiveProfile(string profileId)
    {
        var profile = Profiles.FirstOrDefault(candidate => candidate.Id == profileId) ?? Profiles.First();

        ActiveProfileId = profile.Id;
        ActiveProfileName = profile.Name;

        foreach (var item in Profiles)
        {
            item.IsActive = item.Id == profile.Id;
        }

        ValidationItems.Clear();

        switch (profile.Id)
        {
            case "casual-coop":
                HeadlineStatus = "Ready to launch after a quick review";
                LaunchReadiness = "1 warning · 0 blockers";
                WarningCount = 1;
                BlockerCount = 0;
                ValidationItems.Add(new ValidationItem("Warning", "DiscordPresence.dll is disabled", "Optional DLL can stay off unless you want rich presence."));
                ValidationItems.Add(new ValidationItem("Info", "Seamless Co-op root is enabled", "Expected for the active profile."));
                break;
            case "randomizer-lab":
                HeadlineStatus = "Launch blocked until hard conflicts are resolved";
                LaunchReadiness = "2 blockers · 1 warning";
                WarningCount = 1;
                BlockerCount = 2;
                ValidationItems.Add(new ValidationItem("Blocker", "Randomizer and Reforged UI both replace regulation.bin", "Disable one file-mod root or swap to separate profiles."));
                ValidationItems.Add(new ValidationItem("Blocker", "Missing required game path confirmation", "Re-run path validation before launch."));
                ValidationItems.Add(new ValidationItem("Warning", "FPSUnlocker.dll uses an extended delay", "Keep if you rely on delayed injection ordering."));
                break;
            default:
                HeadlineStatus = "Vanilla fallback is safe to launch";
                LaunchReadiness = "0 warnings · 0 blockers";
                WarningCount = 0;
                BlockerCount = 0;
                ValidationItems.Add(new ValidationItem("Info", "No file mods enabled", "Vanilla recovery path."));
                ValidationItems.Add(new ValidationItem("Info", "No DLL mods enabled", "Safe mode is clean."));
                break;
        }
    }

    public void UseSafeRecoveryProfile() => SetActiveProfile("vanilla");

    public void RunPrototypeValidation()
    {
        SetActiveProfile(ActiveProfileId);
        SelectedLogText = $"[Prototype validation]\nProfile: {ActiveProfileName}\nStatus: {LaunchReadiness}\n\nThis prototype simulates the final validation panel without touching a real Elden Ring install.";
    }

    public ImportedArchiveItem ImportArchive(string archivePath, ModImportTarget target)
    {
        var imported = CreateImportedArchiveItem(archivePath, target, target == ModImportTarget.FileMod ? FileMods.Select(item => item.Id) : DllMods.Select(item => item.Id));
        ApplyImportedArchive(imported);
        LastImportMessage = $"Imported 1 archive into {GetTargetLabel(target)}.";
        AppendImportLog(new[] { imported }, target);
        return imported;
    }

    public IReadOnlyList<ImportedArchiveItem> ImportArchives(IEnumerable<string> archivePaths, ModImportTarget target)
    {
        var existingIds = new HashSet<string>(target == ModImportTarget.FileMod ? FileMods.Select(item => item.Id) : DllMods.Select(item => item.Id), StringComparer.OrdinalIgnoreCase);
        var importedItems = new List<ImportedArchiveItem>();

        foreach (var archivePath in archivePaths)
        {
            var imported = CreateImportedArchiveItem(archivePath, target, existingIds);
            importedItems.Add(imported);
            existingIds.Add(imported.Id);
        }

        if (importedItems.Count == 0)
        {
            LastImportMessage = $"No archives imported into {GetTargetLabel(target)}.";
            return importedItems;
        }

        foreach (var importedItem in importedItems)
        {
            ApplyImportedArchive(importedItem);
        }

        LastImportMessage = $"Imported {importedItems.Count} archive{(importedItems.Count == 1 ? string.Empty : "s")} into {GetTargetLabel(target)}.";
        AppendImportLog(importedItems, target);
        return importedItems;
    }

    public void SelectLog(LogEntry entry)
    {
        SelectedLogText = entry.Content;
    }

    private ImportedArchiveItem CreateImportedArchiveItem(string archivePath, ModImportTarget target, IEnumerable<string> existingIds)
    {
        ValidateArchivePath(archivePath);

        var archiveName = ExtractFileName(archivePath);
        var baseName = ExtractFileNameWithoutExtension(archivePath);
        var uniqueId = NextUniqueId(Slugify(baseName), existingIds);

        return new ImportedArchiveItem
        {
            Id = uniqueId,
            Name = HumanizeName(baseName),
            ArchiveFileName = archiveName,
            Path = target == ModImportTarget.FileMod
                ? $@"imports\filemods\{archiveName}"
                : $@"imports\dllmods\{archiveName}",
            Scope = target == ModImportTarget.FileMod ? "Archive" : string.Empty,
            Notes = target == ModImportTarget.FileMod ? $"Imported from archive ({archiveName})" : string.Empty,
            Status = target == ModImportTarget.DllMod ? "Imported archive" : string.Empty,
            Target = target
        };
    }

    private void ApplyImportedArchive(ImportedArchiveItem imported)
    {
        if (imported.Target == ModImportTarget.FileMod)
        {
            FileMods.Add(new FileModItem
            {
                Id = imported.Id,
                Name = imported.Name,
                Path = imported.Path,
                Scope = imported.Scope,
                Notes = imported.Notes,
                Enabled = true
            });

            return;
        }

        DllMods.Add(new DllModItem
        {
            Id = imported.Id,
            Name = imported.Name,
            Path = imported.Path,
            Enabled = true,
            Required = false,
            LoadOrder = DllMods.Count + 1,
            LoadDelayMs = 1000,
            Status = imported.Status
        });
    }

    private void AppendImportLog(IReadOnlyList<ImportedArchiveItem> importedItems, ModImportTarget target)
    {
        var lines = importedItems.Select(item => $"- {item.ArchiveFileName} -> {item.Path}");
        var content = $"[Prototype import]\nTarget: {GetTargetLabel(target)}\nCount: {importedItems.Count}\n{string.Join("\n", lines)}";
        var logEntry = new LogEntry
        {
            Title = $"Import · {GetTargetLabel(target)}",
            Category = "import.log",
            Content = content
        };

        Logs.Insert(0, logEntry);
        SelectedLogText = content;
    }

    private static void ValidateArchivePath(string archivePath)
    {
        if (string.IsNullOrWhiteSpace(archivePath))
        {
            throw new ArgumentException("A mod archive path is required.", nameof(archivePath));
        }

        var extension = Path.GetExtension(archivePath);
        if (!SupportedArchiveExtensions.Contains(extension))
        {
            throw new ArgumentException("The selected file must be a supported archive (.zip, .7z, .rar).", nameof(archivePath));
        }
    }

    private static string GetTargetLabel(ModImportTarget target) => target == ModImportTarget.FileMod ? "File Mods" : "DLL Mods";

    private static string NextUniqueId(string baseId, IEnumerable<string> existingIds)
    {
        var ids = new HashSet<string>(existingIds, StringComparer.OrdinalIgnoreCase);
        if (!ids.Contains(baseId))
        {
            return baseId;
        }

        var suffix = 2;
        while (ids.Contains($"{baseId}-{suffix}"))
        {
            suffix++;
        }

        return $"{baseId}-{suffix}";
    }

    private static string Slugify(string value)
    {
        var chars = value
            .Trim()
            .ToLowerInvariant()
            .Select(character => char.IsLetterOrDigit(character) ? character : '-')
            .ToArray();

        var slug = string.Join("-", new string(chars)
            .Split('-', StringSplitOptions.RemoveEmptyEntries));

        return string.IsNullOrWhiteSpace(slug) ? "imported-archive" : slug;
    }

    private static string HumanizeName(string value)
    {
        var normalized = value.Replace('_', ' ').Replace('-', ' ');
        return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(normalized.ToLowerInvariant());
    }

    private static string ExtractFileName(string path)
    {
        var normalized = path.Replace('\\', '/');
        return normalized.Split('/', StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? string.Empty;
    }

    private static string ExtractFileNameWithoutExtension(string path)
    {
        var fileName = ExtractFileName(path);
        var extension = Path.GetExtension(fileName);
        return string.IsNullOrEmpty(extension)
            ? fileName
            : fileName[..^extension.Length];
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public enum ModImportTarget
{
    FileMod,
    DllMod
}

public sealed class ImportedArchiveItem
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string ArchiveFileName { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
    public string Scope { get; init; } = string.Empty;
    public string Notes { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public ModImportTarget Target { get; init; }
}

public sealed class ProfileItem : NotifyBase
{
    private bool _isActive;

    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ReadyLabel { get; set; } = string.Empty;
    public bool IsSafe { get; set; }

    public bool IsActive
    {
        get => _isActive;
        set => SetField(ref _isActive, value);
    }
}

public sealed class FileModItem : NotifyBase
{
    private bool _enabled;

    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    public bool Enabled
    {
        get => _enabled;
        set => SetField(ref _enabled, value);
    }
}

public sealed class DllModItem : NotifyBase
{
    private bool _enabled;

    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public bool Required { get; set; }
    public int LoadOrder { get; set; }
    public int LoadDelayMs { get; set; }
    public string Status { get; set; } = string.Empty;

    public bool Enabled
    {
        get => _enabled;
        set => SetField(ref _enabled, value);
    }
}

public sealed class ValidationItem
{
    public ValidationItem(string severity, string issue, string suggestedFix)
    {
        Severity = severity;
        Issue = issue;
        SuggestedFix = suggestedFix;
    }

    public string Severity { get; }
    public string Issue { get; }
    public string SuggestedFix { get; }
}

public sealed class LogEntry
{
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public abstract class NotifyBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
