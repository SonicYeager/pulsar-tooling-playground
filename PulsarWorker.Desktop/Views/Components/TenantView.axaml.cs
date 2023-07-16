using Avalonia.ReactiveUI;
using PulsarWorker.Desktop.ViewModels;

namespace PulsarWorker.Desktop.Views.Components;

public sealed partial class TenantView : ReactiveUserControl<ViewModelBase>
{
    public TenantView()
    {
        InitializeComponent();
    }
}