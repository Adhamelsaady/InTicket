using Microsoft.EntityFrameworkCore;

namespace InTicket.Domain;

public class Payments
{
    public Guid PaymentId { get;  set; }
    public int Price { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool Done { get; set; }
    public List<Guid> TicketIds { get; set; } = new List<Guid>();
    
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}