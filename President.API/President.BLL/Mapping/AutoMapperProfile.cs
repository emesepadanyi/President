using AutoMapper;
using President.BLL.Dtos;
using President.DAL.Entities;

namespace President.BLL.Mapping
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
