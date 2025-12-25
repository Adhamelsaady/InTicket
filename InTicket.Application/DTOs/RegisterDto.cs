using System.ComponentModel.DataAnnotations;

namespace InTicket.Application.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Full name is required")]
    [MinLength(2, ErrorMessage = "Full name must be at least 2 characters")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Full name is required")]
    [MinLength(2, ErrorMessage = "Full name must be at least 2 characters")]
    public string LastName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Full name is required")]
    [MinLength(2, ErrorMessage = "Full name must be at least 2 characters")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "PhoneNumber is required")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "National Id is required")]
    [Length(14 , 14 , ErrorMessage = "Enter a valid National Id") ]
    public string NationalId { get; set; } = string.Empty;
}