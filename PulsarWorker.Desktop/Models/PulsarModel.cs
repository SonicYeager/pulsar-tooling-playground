using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using PulsarWorker.Client;
using PulsarWorker.Desktop.Services;

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

    public async Task<IEnumerable<string>> GetClusters(Action<string, NotificationType, string> onNotify)
    {
        return await HandleResponse(
            async () => await _pulsarClient.GetClusters(),
            onNotify);
    }

    public async Task<IEnumerable<string>> GetTenants(Action<string, NotificationType, string> onNotify)
    {
        return await HandleResponse(
            async () => await _pulsarClient.GetTenants(),
            onNotify);
    }

    public async Task<IEnumerable<string>> GetNameSpaces(string? tenant, Action<string, NotificationType, string> onNotify)
    {
        return await HandleResponse(
            async () => await _pulsarClient.GetNamespaces(tenant),
            onNotify);
    }
    public async Task<IEnumerable<string>> GetTopics(string? tenant, string? nameSpace,
        Action<string, NotificationType, string> onNotify)
    {
        return await HandleResponse(
            async () => await _pulsarClient.GetTopics(tenant, nameSpace),
            onNotify);
    }

    private static async Task<IEnumerable<string>> HandleResponse(
        Func<Task<IEnumerable<string>?>> fetchFromApi,
        Action<string, NotificationType, string> onNotify)
    {
        var viewModels = new List<string>();

        try
        {
            viewModels.AddRange(await fetchFromApi());
        }
        catch (Exception e)
        {
            onNotify("Error", NotificationType.Error, e.Message);
            //TODO show error page upon exception -> use event
        }

        return viewModels;
    }
}