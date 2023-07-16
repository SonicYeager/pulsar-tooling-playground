using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Microsoft.Extensions.DependencyInjection;
using PulsarWorker.Client;
using PulsarWorker.Desktop.Services;
using PulsarWorker.Desktop.ViewModels.Components;

namespace PulsarWorker.Desktop.Models;

public sealed class PulsarModel
{
    private readonly IPulsarClient _pulsarClient;
    private readonly SettingsManager _settingsManager;
    private readonly IServiceProvider _serviceProvider;

    public PulsarModel(IPulsarClient pulsarClient, SettingsManager settingsManager, IServiceProvider serviceProvider)
    {
        _pulsarClient = pulsarClient;
        _settingsManager = settingsManager;
        _serviceProvider = serviceProvider;

        _settingsManager.OnSettingChanged += (key, value) =>
        {
            if (key == AvailableSettings.PulsarHostOptionKey && value is string hostAddress && !string.IsNullOrEmpty(hostAddress))
                _pulsarClient.ChangeBaseAddress(new(hostAddress));
        };
    }

    public async Task<IEnumerable<ClusterViewModel>> GetClusters(Action<string, NotificationType, string> onNotify)
    {
        var clusterViewModels = new List<ClusterViewModel>();

        try
        {
            foreach (var clusterName in await _pulsarClient.GetClusters())
            {
                var clusterViewModel = _serviceProvider.GetRequiredService<ClusterViewModel>();
                clusterViewModel.Name = clusterName;
                clusterViewModels.Add(clusterViewModel);
            }
        }
        catch (Exception e)
        {
            onNotify("Error", NotificationType.Error, e.Message);
            //TODO show error page upon exception -> use event
        }

        return clusterViewModels;
    }

    public async Task<IEnumerable<TenantViewModel>> GetTenants(Action<string, NotificationType, string> onNotify)
    {
        var clusterViewModels = new List<TenantViewModel>();

        try
        {
            foreach (var clusterName in await _pulsarClient.GetTenants())
            {
                var clusterViewModel = _serviceProvider.GetRequiredService<TenantViewModel>();
                clusterViewModel.Name = clusterName;
                clusterViewModels.Add(clusterViewModel);
            }
        }
        catch (Exception e)
        {
            onNotify("Error", NotificationType.Error, e.Message);
            //TODO show error page upon exception -> use event
        }

        return clusterViewModels;
    }
}