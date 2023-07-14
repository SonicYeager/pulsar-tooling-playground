using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using PulsarWorker.Desktop.ViewModels;
using ReactiveUI;

namespace PulsarWorker.Desktop.Views.Components;

public sealed partial class TopicView : ReactiveUserControl<ViewModelBase>
{
    public TopicView()
    {
        InitializeComponent();
    }
}