namespace InTicket.Application.Feauters.Profile.Queries.GetMyPaymenrs;

public class GetPaymentResponse 
{
    public Guid PaymentId { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int Price {get; set;}
    public bool Done {get; set;}
    public int TicketsCount {get; set;}
}