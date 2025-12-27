using AutoMapper;
using InTicket.Application.Feauters.Authentication.Register;
using InTicket.Domain;

namespace InTicket.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, RegisterCommand>().ReverseMap();
    }
}
