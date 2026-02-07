using System.ComponentModel.DataAnnotations;

namespace InTicket.Domain.Dtos;

public class MatchTicketForBookingDto
{
    [Required]
    public string BookingForUserId {get; set;}
    [Required]
    public bool isHomeTeam {get; set;}
    [Required]
    public MatchTicketClass Class {get; set;}
}