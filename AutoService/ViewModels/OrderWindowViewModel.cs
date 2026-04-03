using System;
using System.Collections.Generic;
using AutoService.DB;
using AutoService.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AutoService.ViewModels;

public partial class OrderWindowViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly OrderRepository _orderRepository;

    [ObservableProperty] List<Work> _works;
    [ObservableProperty] decimal _totalPrice;
    [ObservableProperty] decimal _worksSum;
    [ObservableProperty] int _discountPercent;
    [ObservableProperty] Service _service;
    [ObservableProperty] private string _clientName;
    [ObservableProperty] private string _carModel;
    
    public OrderWindowViewModel(IServiceProvider serviceProvider, OrderRepository orderRepository, List<Work> works, string clientName, string carModel, Service service)
    {
        _serviceProvider = serviceProvider;
        _orderRepository = orderRepository;
        _works = works;
        _clientName = clientName;
        _carModel = carModel;
        _service = service;
        
        GetTotalPrice();
    }

    public void GetTotalPrice()
    {
        for (int i = 0; i < _works.Count; i++)
        {
            WorksSum += _works[i].Price;
        }

        TotalPrice = WorksSum;

        if (TotalPrice >= 10000)
        {
            TotalPrice *= 0.9m;
            DiscountPercent = 10;
        }
        else if (TotalPrice >= 5000)
        {
            TotalPrice *= 0.95m;
            DiscountPercent = 5;
        }
    }

    [RelayCommand]
    public void SaveCommand()
    {
        Order order = new Order();
        order.ClientName = _clientName;
        order.CarModel = _carModel;
        order.ServiceId = _service.Id;
        order.OrderDate = DateTime.Now;
        order.TotalAmount = TotalPrice;
        order.DiscountPercent = DiscountPercent;
        
        _orderRepository.InsertOrder(order, Works);
    }
}