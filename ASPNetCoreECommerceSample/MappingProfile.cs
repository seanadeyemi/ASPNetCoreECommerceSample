using ASPNetCoreECommerceSample.Entities.Identity;
using ASPNetCoreECommerceSample.Models;
using AutoMapper;

namespace ASPNetCoreECommerceSample
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserRegistrationModel>().ForMember(u => u.Email, opt => opt.MapFrom(x => x.Email)
            ).ForMember(u => u.UserName, opt => opt.MapFrom(x => x.UserName))
            .ForMember(u => u.FirstName, opt => opt.MapFrom(x => x.FirstName))
            .ForMember(u => u.LastName, opt => opt.MapFrom(x => x.LastName));


            CreateMap<UserRegistrationModel, ApplicationUser>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.UserName))
                .ForMember(u => u.Email, opt => opt.MapFrom(x => x.Email))
                .ForMember(u => u.FirstName, opt => opt.MapFrom(x => x.FirstName))
                .ForMember(u => u.LastName, opt => opt.MapFrom(x => x.LastName))

                ;
        }
    }
}
