using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;

namespace PulsarWorker.Desktop.ViewModels;

public class ViewModelBase : ReactiveObject
{
    public virtual void HandleActivation(CompositeDisposable disposables)
    {
    }

    public virtual void HandleDeactivation()
    {
    }
}