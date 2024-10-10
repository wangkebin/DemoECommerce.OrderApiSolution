using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs;

public record AppUserDTO(
    int Id,
    [Required] string Name,
    [Required, EmailAddress] string Email,
    [Required] string Password,
    [Required] string PhoneNumber,
    [Required] string Address,
    [Required] string Role
    );