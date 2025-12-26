namespace InTicket.Domain;
using Microsoft.AspNetCore.Identity;
public class ApplicationUser : IdentityUser
{
    public string NationalId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Guid InTicketId { get; set; }
    public bool IsAdmin { get; set; }
    
    public string? EmailConfirmationOtp { get; set; }
    public DateTime? EmailConfirmationOtpExpiration { get; set; }
    public string? PasswordResetOtp { get; set; }
    public DateTime? PasswordResetOtpExpiration { get; set; }
    public int OtpAttempts { get; set; } = 0;
    public DateTime? LastOtpAttemptAt { get; set; }
}