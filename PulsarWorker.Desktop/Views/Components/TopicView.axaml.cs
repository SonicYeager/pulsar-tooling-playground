using Avalonia.ReactiveUI;
using PulsarWorker.Desktop.ViewModels;

namespace PulsarWorker.Desktop.Views.Components;

public sealed partial class TopicView : ReactiveUserControl<ViewModelBase>
{
    public TopicView()
    {
        InitializeComponent();
    }
}