using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoService.DB;
using AutoService.Models;
using AutoService.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AutoService.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IServiceProvider _provider;
    [ObservableProperty] string clientName;
    [ObservableProperty] string carModel;
    [ObservableProperty] List<Service> services;
    [ObservableProperty] Service selectedService;

    public MainWindowViewModel(IServiceProvider serviceProvider, ServiceRepository serviceRepository)
    {
        _provider = serviceProvider;
        services = serviceRepository.GetAll();
    }

    [RelayCommand]
    public void OpenWorksWindow()
    {
        if (SelectedService == null)
            return;
        var vm = ActivatorUtilities.CreateInstance<WorksWindowViewModel>(
            _provider, 
            SelectedService, CarModel);
        vm.ClientName = clientName;
        var win = _provider.GetRequiredService<WorksWindow>();
        win.DataContext = vm;
        win.Show();

    }
}