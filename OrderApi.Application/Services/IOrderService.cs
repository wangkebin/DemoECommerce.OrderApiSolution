using OrderApi.Application.DTOs;

namespace OrderApi.Application.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderDTO>> GetOrdersByCustomerIdAsync(int customerId);
    Task<OrderDetailDTO> GetOrderDetailsAsync(Guid orderId);
}