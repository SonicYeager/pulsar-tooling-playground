using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using PulsarWorker.Desktop.ViewModels;
using PulsarWorker.Desktop.ViewModels.Components;
using ReactiveUI;

namespace PulsarWorker.Desktop.Views.Components;

public sealed partial class ClusterView : ReactiveUserControl<ViewModelBase>
{
    public ClusterView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            ViewModel?.HandleActivation(disposables);

            if (ViewModel != null)
                Disposable
                    .Create(ViewModel.HandleDeactivation)
                    .DisposeWith(disposables);
        });
    }
}