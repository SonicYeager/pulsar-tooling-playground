using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Selection;
using Avalonia.Threading;
using DynamicData;
using Microsoft.Extensions.DependencyInjection;
using PulsarWorker.Desktop.Models;
using ReactiveUI;
using Notification = Avalonia.Controls.Notifications.Notification;

namespace PulsarWorker.Desktop.ViewModels;

public sealed class PulsarApiViewModel : ViewModelBase
{
    public ObservableCollection<string> Clusters { get; init; } = new();
    public string SelectedCluster
    {
        get => _selectedCluster;
        set => this.RaiseAndSetIfChanged(ref _selectedCluster, value);
    }
    private string _selectedCluster = string.Empty;
    public ObservableCollection<string> Tenants { get; init; } = new();
    public string SelectedTenant
    {
        get => _selectedTenant;
        set => this.RaiseAndSetIfChanged(ref _selectedTenant, value);
    }
    private string _selectedTenant = string.Empty;
    public ObservableCollection<string> NameSpaces { get; init; } = new();
    public string SelectedNameSpace
    {
        get => _selectedNameSpace;
        set => this.RaiseAndSetIfChanged(ref _selectedNameSpace, value);
    }
    private string _selectedNameSpace = string.Empty;
    public ObservableCollection<string> Topics { get; init; } = new();
    public string SelectedTopic
    {
        get => _selectedTopic;
        set => this.RaiseAndSetIfChanged(ref _selectedTopic, value);
    }
    private string _selectedTopic = string.Empty;

    private readonly IServiceProvider _provider;
    private readonly PulsarModel _model;
    private IManagedNotificationManager? ManagedNotificationManager { get; set; }

    public PulsarApiViewModel(PulsarModel model, IServiceProvider provider)
    {
        _model = model;
        _provider = provider;
        this.WhenActivated(disposables =>
        {
            Observable.StartAsync(async () => await model.GetClusters(Notify))
                //.ObserveOn(RxApp.MainThreadScheduler) // schedule back to main scheduler only if the 'stuff to do' is on ui thread
                .Subscribe(LoadClusters)
                .DisposeWith(disposables);

            Observable.StartAsync(async () => await model.GetTenants(Notify))
                //.ObserveOn(RxApp.MainThreadScheduler) // schedule back to main scheduler only if the 'stuff to do' is on ui thread
                .Subscribe(LoadTenants)
                .DisposeWith(disposables);

            //Disposable
            //    .Create(() => { /* handle deactivation */ })
            //    .DisposeWith(disposables);
        });

        this.WhenAnyValue(static t => t.SelectedTenant)
            .SelectMany(async x =>
            {
                if (!string.IsNullOrEmpty(x))
                {
                    NameSpaces.Clear();
                    SelectedNameSpace = string.Empty;
                    return await _model.GetNameSpaces(x, Notify);
                }

                return default!;
            })
            .Subscribe(LoadNameSpaces);
        this.WhenAnyValue(static t => t.SelectedNameSpace, static t => t.SelectedTenant)
            .SelectMany(async props =>
            {
                if (!string.IsNullOrEmpty(props.Item1) && !string.IsNullOrEmpty(props.Item2))
                {
                    Topics.Clear();
                    SelectedTopic = string.Empty;
                    return await _model.GetTopics(props.Item2, props.Item1, Notify);
                }

                return default!;
            })
            .Subscribe(LoadTopics);
    }

    private void LoadClusters(IEnumerable<string> clusters)
    {
        foreach (var cluster in clusters)
        {
            Clusters.Add(cluster);
        }
    }

    private void LoadTenants(IEnumerable<string> tenants)
    {
        foreach (var tenant in tenants)
        {
            Tenants.Add(tenant);
        }
    }

    private void LoadNameSpaces(IEnumerable<string>? nameSpaces)
    {
        if (nameSpaces != null)
            NameSpaces.AddRange(nameSpaces);
    }

    private void LoadTopics(IEnumerable<string>? topics)
    {
        if (topics != null)
            Topics.AddRange(topics);
    }

    private async void Notify(string title, NotificationType notificationType, string message)
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            ManagedNotificationManager ??= _provider.GetRequiredService<IManagedNotificationManager>();
            ManagedNotificationManager?.Show(new Notification(title, message, notificationType));
        }).GetTask();
    }
}