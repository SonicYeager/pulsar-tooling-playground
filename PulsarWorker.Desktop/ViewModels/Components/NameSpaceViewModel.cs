using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using PulsarWorker.Desktop.Models;
using ReactiveUI;

namespace PulsarWorker.Desktop.ViewModels.Components;

public sealed class NameSpaceViewModel : ViewModelBase
{
    public ObservableCollection<ViewModelBase> Topics { get; init; } = new();
    public string Name { get; set; } = string.Empty;
    public string Tenant { get; set; } = string.Empty;

    private readonly IServiceProvider _provider;
    private IManagedNotificationManager? _managedNotificationManager;

    public NameSpaceViewModel(PulsarModel model, IServiceProvider provider)
    {
        _provider = provider;
        this.WhenActivated(disposables =>
        {
            Observable.StartAsync(async () => await model.GetTopics(Tenant, Name, Notify))
                //.ObserveOn(RxApp.MainThreadScheduler) // schedule back to main scheduler only if the 'stuff to do' is on ui thread
                .Subscribe(LoadAsync)
                .DisposeWith(disposables);
            //Disposable
            //    .Create(() => { /* handle deactivation */ })
            //    .DisposeWith(disposables);
        });
    }

    private void LoadAsync(IEnumerable<TopicViewModel> viewModels)
    {
        foreach (var cluster in viewModels)
        {
            Topics.Add(cluster);
        }
    }

    private async void Notify(string title, NotificationType notificationType, string message)
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            _managedNotificationManager ??= _provider.GetRequiredService<IManagedNotificationManager>();
            _managedNotificationManager?.Show(new Notification(title, message, notificationType));
        }).GetTask();
    }
}