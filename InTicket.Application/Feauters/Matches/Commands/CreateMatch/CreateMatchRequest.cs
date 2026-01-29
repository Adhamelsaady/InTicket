using System.ComponentModel.DataAnnotations;
using InTicket.Domain;
using MediatR;

namespace InTicket.Application.Feauters.Matches.Commands.CreateMatch;

public class CreateMatchRequest : IRequest<CreateMatchRequestResponse>
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(2000)]
    public string? Description { get; set; }
    
    [Required]
    public DateTime EventDate { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Venue { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;
    
    [Required]
    public int Price { get; set; }
    
    [Required]
    public int TotalCapacity { get; set; }
    
    [Required]
    public int AvailableTickets { get; set; }
    
    [Required]
    public Guid HomeTeamId { get; set; }

    [Required] public Guid AwayTeamId { get; set; } 
    
    [Required]
    [MaxLength(100)]
    public string League { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(20)]
    public string Season { get; set; } = string.Empty;  
    
    [Required]
    public int? Round { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string StadiumName { get; set; } = string.Empty;
    
    [Required]
    public int StadiumCapacity { get; set; }
    
    [Required] 
    public DateTime GeneralBookingStart { get; set; }
    
        [Required]
        public Dictionary<MatchTicketClass , TicketData> TicketsDistribution { get; set; } 
            = new Dictionary<MatchTicketClass , TicketData> ();
}