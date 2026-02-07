using AutoMapper;
using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Application.Feauters.Matches.Commands.CreateMatch;
using InTicket.Application.Feauters.Matchs.Queries;
using InTicket.Application.Feauters.Matchs.Queries.Common;
using InTicket.Application.Feauters.Profile.Queries.GetMyPaymenrs;
using InTicket.Domain;

namespace InTicket.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, RegisterCommand>().ReverseMap();
        CreateMap<Match, CreateMatchRequest>().ReverseMap();
        CreateMap<Match, GetMatchResponse>();
        CreateMap<TeamDto, Team>().ReverseMap();
        CreateMap<Payments , GetPaymentResponse>().ForMember(dest => dest.TicketsCount, 
            opt => opt.MapFrom(src => src.TicketIds.Count));
    }
}
