using ReactiveUI;

namespace PulsarWorker.Desktop.ViewModels;

public class ViewModelBase : ReactiveObject, IActivatableViewModel
{
    public ViewModelBase()
    {
        Activator = new ViewModelActivator();
    }

    public ViewModelActivator Activator { get; }
}