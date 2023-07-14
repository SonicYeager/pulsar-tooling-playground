using System.Collections.ObjectModel;

namespace PulsarWorker.Desktop.ViewModels.Components;

public sealed class ClusterViewModel : ViewModelBase
{
    public ObservableCollection<ViewModelBase> SubNodes { get; set; }
    public string Name { get; set; }
}