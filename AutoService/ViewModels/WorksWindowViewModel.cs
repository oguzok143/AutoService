using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using AutoService.DB;
using AutoService.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AutoService.Models;

public partial class WorksWindowViewModel : ViewModelBase
{
    private readonly WorkRepository _workRepository;
    private readonly Service _service;

    [ObservableProperty] private string _clientName;
    [ObservableProperty] List<Work> _works;
    [ObservableProperty] List<Work> _selectedWorks;

    [ObservableProperty] private bool? select_1;
    [ObservableProperty] private bool? select_2;
    [ObservableProperty] private bool? select_3;
    [ObservableProperty] private bool? select_4;
    [ObservableProperty] private bool? select_5;
    
    public WorksWindowViewModel(WorkRepository repository, Service selectedService)
    {
        _workRepository = repository;
        _service = selectedService;
        
        GetWorks();
    }

    [RelayCommand]
    public void GetWorks()
    {
        Works = _workRepository.GetAll().Where(w => _service.Id == w.ServiceId).ToList();
    }

    [RelayCommand]
    public void NextCommand()
    {
        
    }
}