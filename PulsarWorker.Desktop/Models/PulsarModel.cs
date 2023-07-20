using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Microsoft.Extensions.DependencyInjection;
using PulsarWorker.Client;
using PulsarWorker.Desktop.Services;
using PulsarWorker.Desktop.ViewModels;
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
        return await HandleResponse<ClusterViewModel>(
            async () => await _pulsarClient.GetClusters(),
            node =>
            {
                var clusterViewModel = _serviceProvider.GetRequiredService<ClusterViewModel>();
                clusterViewModel.Name = node;
                return clusterViewModel;
            },
            onNotify);
    }

    public async Task<IEnumerable<TenantViewModel>> GetTenants(Action<string, NotificationType, string> onNotify)
    {
        return await HandleResponse<TenantViewModel>(
            async () => await _pulsarClient.GetTenants(),
            node =>
            {
                var tenantViewModel = _serviceProvider.GetRequiredService<TenantViewModel>();
                tenantViewModel.Name = node;
                return tenantViewModel;
            },
            onNotify);
    }

    public async Task<IEnumerable<NameSpaceViewModel>> GetNameSpaces(string tenant, Action<string, NotificationType, string> onNotify)
    {
        return await HandleResponse<NameSpaceViewModel>(
            async () => await _pulsarClient.GetNamespaces(tenant),
            node =>
            {
                var nameSpaceViewModel = _serviceProvider.GetRequiredService<NameSpaceViewModel>();
                nameSpaceViewModel.Name = node;
                nameSpaceViewModel.Tenant = tenant;
                return nameSpaceViewModel;
            },
            onNotify);
    }
    public async Task<IEnumerable<TopicViewModel>> GetTopics(string tenant, string nameSpace,
        Action<string, NotificationType, string> onNotify)
    {
        return await HandleResponse<TopicViewModel>(
            async () => await _pulsarClient.GetTopics(tenant, nameSpace),
            node =>
            {
                var topicViewModel = _serviceProvider.GetRequiredService<TopicViewModel>();
                topicViewModel.Name = node;
                return topicViewModel;
            },
            onNotify);
    }

    private static async Task<IEnumerable<TViewModel>> HandleResponse<TViewModel>(
        Func<Task<IEnumerable<string>?>> fetchFromApi,
        Func<string, TViewModel> viewModelFactory,
        Action<string, NotificationType, string> onNotify)
        where TViewModel : ViewModelBase
    {
        var viewModels = new List<TViewModel>();

        try
        {
            viewModels.AddRange((await fetchFromApi())!.Select(viewModelFactory));
        }
        catch (Exception e)
        {
            onNotify("Error", NotificationType.Error, e.Message);
            //TODO show error page upon exception -> use event
        }

        return viewModels;
    }
}