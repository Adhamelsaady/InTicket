using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.ResourceParameters;
using InTicket.Domain;

namespace InTicket.Application.Contracts.Presistance;

public interface IConcertsRepository
{
    Task<PagedResult<Concert>> GetAllConcerts(ConcertResourceParameters concertResourceParameters);
}