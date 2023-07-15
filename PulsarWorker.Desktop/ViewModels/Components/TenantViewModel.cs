using System.Collections.ObjectModel;

namespace PulsarWorker.Desktop.ViewModels.Components;

public sealed class TenantViewModel : ViewModelBase
{
    public ObservableCollection<ViewModelBase> NameSpaces { get; set; } = new();
    public string Name { get; set; }
}