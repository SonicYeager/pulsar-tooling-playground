﻿using System;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace PulsarWorker.Desktop.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;

    private bool _paneOpen = false;
    public bool PaneState
    {
        get => _paneOpen;
        set => this.RaiseAndSetIfChanged(ref _paneOpen, value);
    }

    public ICommand ShowSettings { get; }
    public ICommand ShowApi { get; }
    public ICommand TogglePane { get; }

    private ViewModelBase _content = new();

    public ViewModelBase Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }

    //public Interaction<MusicStoreViewModel, AlbumViewModel?> ShowDialog { get; }

    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        TogglePane = ReactiveCommand.Create(() => { PaneState = !PaneState; });
        ShowSettings = ReactiveCommand.Create(() =>
        {
            Content = _serviceProvider.GetRequiredService<SettingsViewModel>();
        });
        ShowApi = ReactiveCommand.Create(() =>
        {
            Content = _serviceProvider.GetRequiredService<PulsarApiViewModel>();
        });

        //ShowDialog = new Interaction<MusicStoreViewModel, AlbumViewModel?>();

        //RxApp.MainThreadScheduler.Schedule(LoadAlbums);
    }
}