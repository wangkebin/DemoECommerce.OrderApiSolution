namespace OrderApi.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public int PurchaseQuantity { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
}