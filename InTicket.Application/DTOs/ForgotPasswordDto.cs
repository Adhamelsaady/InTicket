using System.ComponentModel.DataAnnotations;

namespace InTicket.Application.DTOs;

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}