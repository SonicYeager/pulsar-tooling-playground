using Avalonia.Controls;
using Avalonia.ReactiveUI;
using PulsarWorker.Desktop.ViewModels;

namespace PulsarWorker.Desktop.Views;

public sealed partial class MainWindow : ReactiveWindow<ViewModelBase>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}