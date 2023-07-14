using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;
using PulsarWorker.Desktop.ViewModels;
using System;

namespace PulsarWorker.Desktop;

public sealed class ViewLocator : IDataTemplate
{
    private readonly IServiceProvider _serviceProvider;
    public ViewLocator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Control Build(object? data)
    {
        var name = data?.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        var fallbackValue = new TextBlock //TODO provide proper error view (unable to lead or find view xyz or so!)
        {
            Text = "Not Found: " + name,
        };

        if (type != null)
            return _serviceProvider.GetRequiredService(type) as Control ?? fallbackValue;

        return fallbackValue;
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}