using System.Collections.ObjectModel;

namespace PulsarWorker.Desktop.ViewModels.Components;

public sealed class TopicViewModel : ViewModelBase
{
    public TopicViewModel(ObservableCollection<ViewModelBase> subNodes, string name)
    {
        SubNodes = subNodes;
        Name = name;
    }

    public ObservableCollection<ViewModelBase> SubNodes { get; init; }
    public string Name { get; init; }
}