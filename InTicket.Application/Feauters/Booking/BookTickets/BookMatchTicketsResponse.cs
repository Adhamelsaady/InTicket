namespace InTicket.Application.Feauters.Booking.BookTickets;

public class BookMatchTicketsResponse
{
    public bool IsSuccess {get; set;}
    public Guid PaymentCode { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int TotalTickets { get; set; }
    public int TotalPrice { get; set; }
}