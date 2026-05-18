using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;

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
}
