using System.Collections.ObjectModel;

namespace PulsarWorker.Desktop.ViewModels.Components;

public sealed class TopicViewModel : ViewModelBase
{
    public string Name { get; set; } = string.Empty;
}