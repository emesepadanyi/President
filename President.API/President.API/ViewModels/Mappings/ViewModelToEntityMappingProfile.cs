using AutoMapper;
using President.DAL.Entities;

namespace President.API.ViewModels.Mapping
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public ViewModelToEntityMappingProfile()
        {
            CreateMap<RegistrationViewModel, User>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.UserName));
        }
    }
}
