using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using PulsarWorker.Desktop.Models;
using PulsarWorker.Desktop.ViewModels.Components;
using ReactiveUI;

namespace PulsarWorker.Desktop.ViewModels;

public sealed class PulsarApiViewModel : ViewModelBase
{
    private readonly PulsarModel _model;
    private readonly IServiceProvider _provider;
    private IManagedNotificationManager? ManagedNotificationManager { get; set; }

    public PulsarApiViewModel(PulsarModel model, IServiceProvider provider)
    {
        _model = model;
        _provider = provider;
    }

    public override void HandleActivation(CompositeDisposable disposable)
    {
        base.HandleActivation(disposable);
        Observable.Start(() => _model.GetClusters(Notify).Result)
            //.ObserveOn(RxApp.MainThreadScheduler) // schedule back to main scheduler only if the 'stuff to do' is on ui thread
            .Subscribe(LoadAsync)
            .DisposeWith(disposable);
    }

    private void LoadAsync(IEnumerable<ClusterViewModel> viewModels)
    {
        foreach (var cluster in viewModels)
        {
            Clusters.Add(cluster);
        }
    }

    private async void Notify(string title, NotificationType notificationType, string message)
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            ManagedNotificationManager ??= _provider.GetRequiredService<IManagedNotificationManager>();
            ManagedNotificationManager?.Show(new Notification(title, message, notificationType));
        }).GetTask();
    }

    public ObservableCollection<ViewModelBase> Clusters { get; init; } = new();

}