using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using PulsarWorker.Desktop.Models;
using PulsarWorker.Desktop.ViewModels.Components;

namespace PulsarWorker.Desktop.ViewModels;

public sealed class PulsarApiViewModel : ViewModelBase
{
    private PulsarTreeModel _treeModel;
    private readonly IServiceProvider _provider;
    private IManagedNotificationManager? ManagedNotificationManager { get; set; }

    public PulsarApiViewModel(PulsarTreeModel treeModel, IServiceProvider provider)
    {
        _treeModel = treeModel;
        _provider = provider;
    }

    public async Task LoadAsync() //TODO look into the approach of the music store example for loading
    {
        await _treeModel.GetPulsarNodeTree(Nodes, async (title, type, message) => await Notify(title, type, message));
    }

    private async Task Notify(string title, NotificationType notificationType, string message)
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            ManagedNotificationManager ??= _provider.GetRequiredService<IManagedNotificationManager>();
            ManagedNotificationManager?.Show(new Notification(title, message, notificationType));
        }).GetTask();
    }

    public ObservableCollection<PulsarNodeViewModel> Nodes { get; init; } = new();

}