using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using PulsarWorker.Desktop.ViewModels;
using ReactiveUI;

namespace PulsarWorker.Desktop.Views.Components;

public sealed partial class ClusterView : ReactiveUserControl<ViewModelBase>
{
    public ClusterView()
    {
        InitializeComponent();
    }
}