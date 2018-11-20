using AutoMapper;
using President.API.Dtos;
using President.DAL.Entities;

namespace President.API.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
