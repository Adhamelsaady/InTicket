namespace InTicket.Application.ResourceParameters;

public class PaymentResourceParameters : BaseResourceParameters
{
    public bool? isPaid { get; set; }
    
    public bool? isExpired { get; set; }
}