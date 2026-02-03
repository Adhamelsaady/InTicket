namespace InTicket.Domain.Dtos;

public class MatchTicketForBookingDto
{
    public string BookingForUserId {get; set;}
    public bool isHomeTeam {get; set;}
    public MatchTicketClass Class {get; set;}
}