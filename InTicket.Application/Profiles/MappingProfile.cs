using AutoMapper;
using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.Feauters.Matchs.Queries;
using InTicket.Application.Feauters.Matchs.Queries.Common;
using InTicket.Domain;

namespace InTicket.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, RegisterCommand>().ReverseMap();
        CreateMap<Match, GetMatchResponse>();
        CreateMap<TeamDto, Team>().ReverseMap();
    }
}
