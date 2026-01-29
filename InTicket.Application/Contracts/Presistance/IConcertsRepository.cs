using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.ResourceParameters;
using InTicket.Domain;

namespace InTicket.Application.Contracts.Presistance;

public interface IConcertsRepository
{
    Task <Concert> GetConcertByIdAsync(Guid id ,  bool isRequestedByAdmin);
    Task<PagedResult<Concert>> GetAllConcertsAsync(ConcertResourceParameters concertResourceParameters , bool isRequestedByAdmin);
}