namespace InTicket.Domain;
using Microsoft.AspNetCore.Identity;
public class ApplicationUser : IdentityUser
{
    private string NationalId { get; set; } = string.Empty;
    private string FirstName { get; set; } = string.Empty;
    private string LastName { get; set; } = string.Empty;
    private Guid InTicketId { get; set; }
}