using System.ComponentModel.DataAnnotations;
using MediatR;

namespace InTicket.Application.Feauters.Concerts.Commands.CreateConcert;

public class CreateConcertRequest : IRequest<CreateConcertRequestResponse>
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
    [MaxLength(200)]
    public string Artist { get; set; }  
    
    [Required]
    [MaxLength(200)]
    public string? Organizer { get; set; }  
    
    [Required]
    public int? DurationMinutes { get; set; }
   
    [Required]
    public int? MinimumAge { get; set; }
    

}