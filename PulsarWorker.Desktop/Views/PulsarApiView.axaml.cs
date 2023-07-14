﻿using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using PulsarWorker.Desktop.ViewModels;
using ReactiveUI;

namespace PulsarWorker.Desktop.Views;

public sealed partial class PulsarApiView : ReactiveUserControl<ViewModelBase>
{
    public PulsarApiView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            ViewModel?.HandleActivation(disposables);

            if (ViewModel != null)
                Disposable
                    .Create(ViewModel.HandleDeactivation)
                    .DisposeWith(disposables);
        });
    }
}