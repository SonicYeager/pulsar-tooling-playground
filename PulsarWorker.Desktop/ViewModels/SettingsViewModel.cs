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
using PulsarWorker.Desktop.Services;
using ReactiveUI;

namespace PulsarWorker.Desktop.ViewModels;

public sealed class SettingsViewModel : ViewModelBase
{
    private readonly IServiceProvider _provider;
    private IManagedNotificationManager? ManagedNotificationManager { get; set; }

    public ObservableCollection<ViewModelBase> PersistedOptions { get; init; } = new();

    public SettingsViewModel(SettingsModel model, IServiceProvider provider, UserManager userManager)
    {
        _provider = provider;
        this.WhenActivated(disposables =>
        {
            Observable.StartAsync(async () => await model.GetPersistedSettings(Notify, userManager.CurrentUserId))
                //.ObserveOn(RxApp.MainThreadScheduler) // schedule back to main scheduler only if the 'stuff to do' is on ui thread
                .Subscribe(LoadAsync)
                .DisposeWith(disposables);
            //Disposable
            //    .Create(() => { /* handle deactivation */ })
            //    .DisposeWith(disposables);
        });
    }

    public void LoadAsync(IEnumerable<ViewModelBase> persistedOptions)
    {
        foreach (var option in persistedOptions)
        {
            PersistedOptions.Add(option);
        }
    }


    private async Task Notify()
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            ManagedNotificationManager ??= _provider.GetRequiredService<IManagedNotificationManager>();
            ManagedNotificationManager?.Show(new Notification("SettingsView Saved", "", NotificationType.Success));
        }).GetTask();
    }
}