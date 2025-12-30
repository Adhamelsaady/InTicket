using AutoMapper;
using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.Feauters.Concerts.Commands.CreateConcert;
using InTicket.Application.Feauters.Concerts.Queries.Common;
using InTicket.Application.Feauters.Matches.Commands.CreateMatch;
using InTicket.Application.Feauters.Matchs.Queries;
using InTicket.Application.Feauters.Matchs.Queries.Common;
using InTicket.Domain;

namespace InTicket.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, RegisterCommand>().ReverseMap();
        CreateMap<Match, CreateMatchRequest>().ReverseMap();
        CreateMap<Match, CreateConcertRequest>().ReverseMap();
        CreateMap<Match, GetMatchResponse>();
        CreateMap<Concert, GetConcertResponse>();
        CreateMap<TeamDto, Team>().ReverseMap();
    }
}
