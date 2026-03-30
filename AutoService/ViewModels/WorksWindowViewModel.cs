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
    [ObservableProperty] List<SelectWork> _selectedWorks;
    
    public WorksWindowViewModel(WorkRepository repository, Service selectedService)
    {
        _workRepository = repository;
        _service = selectedService;
        
        GetWorks();
    }

    [RelayCommand]
    public void GetWorks()
    {
        SelectedWorks = _workRepository.GetAll().Where(w => _service.Id == w.ServiceId).Select(s => new SelectWork(s)).ToList();
    }

    [RelayCommand]
    public void NextCommand()
    {
        
    }
}