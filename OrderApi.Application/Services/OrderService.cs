using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using Polly;
using Polly.Registry;

namespace OrderApi.Application.Services;

public class OrderService(IOrder orderInterface, HttpClient httpClient, 
    ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
{
    // GET PRODUCT
    public async Task<ProductDTO> GetProductById(int productId)
    {
        var getProduct = await httpClient.GetAsync($"/api/products/{productId}");
        if (!getProduct.IsSuccessStatusCode)
            return null!;
        var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
        return product!;
    }
    
    // GET USER
    public async Task<AppUserDTO> GetCustomer(int userId)
    {
        var getUser = await httpClient.GetAsync($"/api/user/{userId}");
        if (!getUser.IsSuccessStatusCode)
            return null!;
        var user = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
        return user!;
    }
    public async Task<IEnumerable<OrderDTO>> GetOrdersByCustomerIdAsync(int customerId)
    {
        var orders = await orderInterface.GetAllAsync(o => o.CustomerId == customerId);
        if (!orders.Any()) return null!;
        return OrderConversion.FromOrder(orders)!;
    }

    public async Task<OrderDetailDTO> GetOrderDetailsAsync(Guid orderId)
    {
        var order = await orderInterface.GetByIdAsync(orderId).ConfigureAwait(false);
        if (order is null || order!.Id == Guid.Empty)
        {
            return null!;
        }
        
        // Get Retry pipeline
        var retrypipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");
        
        // Prepare product
        var productDTO = await retrypipeline.ExecuteAsync(async token => await GetProductById(order.ProductId));
        var clientDTO = await retrypipeline.ExecuteAsync( async token => await GetCustomer(order.CustomerId));

        return new OrderDetailDTO(
            CustomerId: order.CustomerId,
            ProductId: order.ProductId,
            OrderId: order.Id,
            Email: clientDTO.Email,
            PhoneNumber:clientDTO.PhoneNumber,
            ProductName:productDTO.Name,
            PurchaseQuantity:order.PurchaseQuantity,
            UnitPrice:productDTO.Price,
            TotalPrice:productDTO.Price * order.PurchaseQuantity,
            OrderDate:order.OrderDate);

    }
}