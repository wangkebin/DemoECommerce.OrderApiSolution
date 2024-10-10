using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs;

public record OrderDetailDTO(
    [Required] int ProductId,
    [Required] int CustomerId,
    [Required] Guid OrderId,
    
    [Required, EmailAddress] string Email,
    [Required] string PhoneNumber,
    [Required] string ProductName,
    [Required] int PurchaseQuantity,    
    [Required, DataType(DataType.Currency)] decimal UnitPrice,
    [Required, DataType(DataType.Currency)] decimal TotalPrice,
    [Required] DateTime OrderDate
    );