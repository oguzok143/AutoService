using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using AutoService.DB;
using AutoService.ViewModels;
using AutoService.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AutoService.Models;

public partial class WorksWindowViewModel : ViewModelBase
{
    private readonly WorkRepository _workRepository;
    private readonly Service _service;
    private readonly IServiceProvider _serviceProvider;
    public string _carModel { get; set; }

    [ObservableProperty] private string _clientName;
    [ObservableProperty] List<SelectWork> _selectedWorks;
    
    public WorksWindowViewModel(IServiceProvider provider, WorkRepository repository, Service selectedService, string carModel)
    {
        _workRepository = repository;
        _service = selectedService;
        _carModel = carModel;
        _serviceProvider = provider;
        
        GetWorks();
    }

    private void GetWorks()
    {
        SelectedWorks = _workRepository.GetAll().Where(w => _service.Id == w.ServiceId).Select(s => new SelectWork(s)).ToList();
    }

    [RelayCommand]
    public void OpenReceiptWindowCommand()
    {
        List<Work> works = new List<Work>();
        for (int i = 0; i < SelectedWorks.Count; i++)
        {
            if (SelectedWorks[i].IsSelected)
            {
                works.Add(SelectedWorks[i].Work);
            }
        }
        
        var vm = ActivatorUtilities.CreateInstance<OrderWindowViewModel>(_serviceProvider, works, ClientName, _carModel, _service);
        vm.ClientName = _clientName;
        var win = _serviceProvider.GetRequiredService<ReceiptWindow>();
        win.DataContext = vm;
        win.Show();
    }
}