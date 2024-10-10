using OrderApi.Domain.Entities;

namespace OrderApi.Application.DTOs.Conversions;

public static class OrderConversion
{
    public static Order ToOrder(this OrderDTO orderDTO) => new Order
    {
        Id = orderDTO.Id,
        CustomerId = orderDTO.CustomerId,
        ProductId = orderDTO.ProductId,
        OrderDate = orderDTO.OrderDate,
        PurchaseQuantity = orderDTO.PurchaseQuantity
    };

    public static OrderDTO? FromOrder(this Order? order)
    {
        if (order is null) return null;
        return new OrderDTO(
            order!.Id,
            order.CustomerId,
            order.ProductId,
            order.PurchaseQuantity,
            order.OrderDate);
    }

    public static IEnumerable<OrderDTO>? FromOrder(this IEnumerable<Order>? orders)
    {
        if (orders is null) return null;
        return orders!.Select(o => new OrderDTO(
                o!.Id,
                o.CustomerId,
                o.ProductId,
                o.PurchaseQuantity,
                o.OrderDate)
        ).ToList();
    }
}