namespace AutoService.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int WorkId { get; set; }
    public decimal WorkPrice { get; set; }
}