using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.ResourceParameters;
using InTicket.Domain;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence.Repositories;

public class ConcertsRepository : IConcertsRepository
{
    private readonly InTicketDbContext _dbContext;

    public ConcertsRepository(InTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<Concert>> GetAllConcerts(ConcertResourceParameters concertResourceParameters)
    {
        var query = _dbContext.Concerts.AsQueryable();
        if (concertResourceParameters == null)
        {
            throw new ArgumentNullException(nameof(concertResourceParameters));
        }
        if (!string.IsNullOrWhiteSpace(concertResourceParameters.SearchQuery))
        {
            query = query.Where(m =>
                m.Title.Contains(concertResourceParameters.SearchQuery) ||
                m.Description.Contains(concertResourceParameters.SearchQuery) ||
                m.Artist.Contains(concertResourceParameters.SearchQuery) ||
                m.Organizer.Contains(concertResourceParameters.SearchQuery));
        }
        
        if (concertResourceParameters.FromDate.HasValue)
        {
            query = query.Where(m => m.EventDate >= concertResourceParameters.FromDate.Value);
        }

        if (concertResourceParameters.ToDate.HasValue)
        {
            query = query.Where(m => m.EventDate <= concertResourceParameters.ToDate.Value);
        }
        var totalCount = await query.CountAsync();
        var concerts = await query
            .Skip((concertResourceParameters.PageNumber - 1) * concertResourceParameters.PageSize)
            .Take(concertResourceParameters.PageSize).ToListAsync();
        return new PagedResult<Concert>
        {
            Items = concerts,
            TotalCount = totalCount,
            PageNumber = concertResourceParameters.PageNumber,
            PageSize = concertResourceParameters.PageSize
        };
    }
}