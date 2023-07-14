using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using PulsarWorker.Desktop.ViewModels;

namespace PulsarWorker.Desktop.Views.Components;

public sealed partial class NameSpaceView : ReactiveUserControl<ViewModelBase>
{
    public NameSpaceView()
    {
        InitializeComponent();
    }
}