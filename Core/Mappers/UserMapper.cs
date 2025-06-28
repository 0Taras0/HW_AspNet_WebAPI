using AutoMapper;
using Domain.Data.Entities.Identity;
using Core.Model.Account;

namespace Core.Interfaces
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<RegisterModel, UserEntity>()
            .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email))
            .ForMember(x => x.Image, opt => opt.Ignore());

            CreateMap<GoogleAccountModel, UserEntity>()
                .ForMember(x => x.Image, opt => opt.Ignore())
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
