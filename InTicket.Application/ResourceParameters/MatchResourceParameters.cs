namespace InTicket.Application.ResourceParameters;

public class MatchResourceParameters : BaseResourceParameters
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public Guid? TeamId { get; set; }  
    public Guid? HomeTeamId { get; set; }
    public Guid? AwayTeamId { get; set; }
    public string? League { get; set; }
    public string? Season { get; set; }
    public string? Stadium { get; set; }
}