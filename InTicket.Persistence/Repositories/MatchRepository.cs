using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.Feauters.Matchs.Queries.Common;
using InTicket.Application.ResourceParameters;
using InTicket.Domain;
using Microsoft.EntityFrameworkCore;

namespace InTicket.Persistence.Repositories;

public class MatchRepository :  BaseRepository<Match>, IMatchRepository
{
    public MatchRepository(InTicketDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Match> GetMatchByIdAsync(Guid matchId)
    {
        var match = await _dbContext.Matches.FindAsync(matchId);
        return match;
    }
    public async Task<PagedResult<Match>> GetAllMatchesAsync(MatchResourceParameters matchResourceParameters)
    {
        var query = _dbContext.Matches
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .AsQueryable();
        if (matchResourceParameters == null)
        {
            throw new ArgumentNullException(nameof(matchResourceParameters));
        }
        if (!string.IsNullOrWhiteSpace(matchResourceParameters.SearchQuery))
        {
            query = query.Where(m =>
                m.Title.Contains(matchResourceParameters.SearchQuery) ||
                m.Description.Contains(matchResourceParameters.SearchQuery) ||
                m.HomeTeam.Name.Contains(matchResourceParameters.SearchQuery) ||
                m.AwayTeam.Name.Contains(matchResourceParameters.SearchQuery));
        }
        
        if (matchResourceParameters.FromDate.HasValue)
        {
            query = query.Where(m => m.EventDate >= matchResourceParameters.FromDate.Value);
        }

        if (matchResourceParameters.ToDate.HasValue)
        {
            query = query.Where(m => m.EventDate <= matchResourceParameters.ToDate.Value);
        }
        
        if (matchResourceParameters.TeamId.HasValue)
        {
            query = query.Where(m =>
                m.HomeTeamId ==  matchResourceParameters.TeamId);
        }

        if (matchResourceParameters.HomeTeamId.HasValue)
        {
            query = query.Where(m => m.HomeTeamId == matchResourceParameters.HomeTeamId.Value);
        }

        if (matchResourceParameters.AwayTeamId.HasValue)
        {
            query = query.Where(m => m.AwayTeamId == matchResourceParameters.AwayTeamId.Value);
        }
        if (!string.IsNullOrWhiteSpace(matchResourceParameters.League))
        {
            query = query.Where(m => m.League.Contains(matchResourceParameters.League));
        }

        if (!string.IsNullOrWhiteSpace(matchResourceParameters.Season))
        {
            query = query.Where(m => m.Season == matchResourceParameters.Season);
        }
        

        if (!string.IsNullOrWhiteSpace(matchResourceParameters.Stadium))
        {
            query = query.Where(m => m.StadiumName.Contains(matchResourceParameters.Stadium));
        }
        var totalCount = await query.CountAsync();
        var matches = await query
            .Skip((matchResourceParameters.PageNumber - 1) * matchResourceParameters.PageSize)
            .Take(matchResourceParameters.PageSize).ToListAsync();

        return new PagedResult<Match>
        {
            Items = matches,
            TotalCount = totalCount,
            PageNumber = matchResourceParameters.PageNumber,
            PageSize = matchResourceParameters.PageSize
        };
    }
    
}