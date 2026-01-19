namespace InTicket.Domain;

public class Delegation
{
    public Guid Id { get; set; }
    public ApplicationUser Delegator { get; set; } = null!;
    public string DelegatorId { get; set; }
    public string DelegateId { get; set; } 
    public ApplicationUser Delegate { get; set; } = null!;
    public string DelegateNationalId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}