using API.Dtos;
using API.Entities;
using AutoMapper;

namespace API.Data.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserToReturnDto>();
            CreateMap<DailyAuthentication, DailyAuthenticationDto>();
        }
    }
}