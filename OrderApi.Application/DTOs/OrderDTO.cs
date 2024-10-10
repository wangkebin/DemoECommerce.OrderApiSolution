using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs;

public record OrderDTO(
    Guid Id,
    [Required] [Range(1, int.MaxValue)] int CustomerId,
    [Required] [Range(1, int.MaxValue)] int ProductId,
    [Required] [Range(1, int.MaxValue)] int PurchaseQuantity,
    DateTime OrderDate
    );