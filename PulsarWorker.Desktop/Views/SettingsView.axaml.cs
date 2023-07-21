using Avalonia.ReactiveUI;
using PulsarWorker.Desktop.ViewModels;

namespace PulsarWorker.Desktop.Views;

public sealed partial class SettingsView : ReactiveUserControl<ViewModelBase>
{
    public SettingsView()
    {
        InitializeComponent();
    }
}