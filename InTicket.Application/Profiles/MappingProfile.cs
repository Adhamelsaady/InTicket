using AutoMapper;
using InTicket.Application.DTOs;
using InTicket.Domain;

namespace InTicket.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, RegisterDto>().ReverseMap();
        
        
    }
}
