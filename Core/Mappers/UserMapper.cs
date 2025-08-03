using AutoMapper;
using Core.Model.Account;
using Domain.Data.Entities.Identity;

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

            CreateMap<AccountUpdateModel, UserEntity>()
                .ForMember(dest => dest.FirstName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.FirstName)))
                .ForMember(dest => dest.LastName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.LastName)))
                .ForMember(dest => dest.Email, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Email)));
        }
    }
}
