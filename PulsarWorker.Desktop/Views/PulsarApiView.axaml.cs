using Avalonia.ReactiveUI;
using PulsarWorker.Desktop.ViewModels;

namespace PulsarWorker.Desktop.Views;

public sealed partial class PulsarApiView : ReactiveUserControl<ViewModelBase>
{
    public PulsarApiView()
    {
        InitializeComponent();
    }
}