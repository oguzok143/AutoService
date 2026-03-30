using System;

namespace AutoService.Models;

public class Order
{
    public int Id { get; set; }
    public string ClientName { get; set; }
    public string CarModel { get; set; }
    public int ServiceId { get; set; }
    public decimal TotalAmount { get; set; }
    public int DiscountPercent { get; set; }
    public DateTime OrderDate { get; set; }
}