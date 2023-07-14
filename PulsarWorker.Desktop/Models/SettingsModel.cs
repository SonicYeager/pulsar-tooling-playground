using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PulsarWorker.Data.Entities;
using PulsarWorker.Database.Context;
using PulsarWorker.Database.Extensions;
using PulsarWorker.Desktop.Services;
using PulsarWorker.Desktop.ViewModels;
using PulsarWorker.Desktop.ViewModels.Components;

namespace PulsarWorker.Desktop.Models;

public sealed class SettingsModel
{
    private readonly DbContextOptions<PulsarWorkerDbContext> _dbContextOptions;
    private readonly SettingsManager _settingsManager;

    private static readonly string[] ThemeChoices;

    public SettingsModel(DbContextOptions<PulsarWorkerDbContext> dbContextOptions, SettingsManager settingsManager)
    {
        _dbContextOptions = dbContextOptions;
        _settingsManager = settingsManager;
    }

    static SettingsModel()
    {
        ThemeChoices = new[]
        {
            "Default", "Dark", "Light",
        };
    }

    public async Task GetPersistedSettings(ObservableCollection<ViewModelBase> observableCollection, Func<Task> onSuccess, int userId)
    {
        await using var context = Repository.Connect(_dbContextOptions);
        var settingsEntities = context.Set<SettingsEntity>().Where(s => s.UserId == userId);
        foreach (var settingsEntity in settingsEntities)
        {
            switch (settingsEntity.Key)
            {
                case AvailableSettings.PulsarHostOptionKey:
                {
                    var textSetting = CreateTextSetting("Pulsar Host", settingsEntity.Value, onSuccess);
                    observableCollection.Add(textSetting);
                    break;
                }
                case AvailableSettings.AppThemeOptionKey:
                {
                    var textSetting = CreateMultipleChoiceSetting("App Theme", ThemeChoices, settingsEntity.Value, onSuccess);
                    observableCollection.Add(textSetting);
                    break;
                }
            }
        }
    }

    private TextSettingViewModel CreateTextSetting(string requestedKey, string? value, Func<Task> onSuccess)
    {
        return new(
            requestedKey,
            async (key, newValue) => await HandleChangedSetting(key, newValue, onSuccess))
        {
            Text = value,
        };
    }

    private MultipleChoiceSettingViewModel CreateMultipleChoiceSetting(
        string requestedKey,
        IEnumerable<string> availableChoices,
        string? value,
        Func<Task> onSuccess)
    {
        return new(requestedKey, availableChoices, value ?? "Default",
            async (key, newValue) => await HandleChangedSetting(key, newValue, onSuccess));
    }

    private async Task HandleChangedSetting(string key, object? value, Func<Task> onSuccess, int userId = 1)
    {
        await PersistSetting(key, value, userId);
        _settingsManager.EmitSettingChanged(key, value);

        await onSuccess();
    }

    private async Task PersistSetting(string key, object? value, int userId)
    {
        await using var context = Repository.Connect(_dbContextOptions);

        var existing = await context.Set<SettingsEntity>().FirstAsync(s => s.UserId == userId && s.Key == key);
        existing.Value = value as string;
        await context.SaveChangesAsync();
    }
}