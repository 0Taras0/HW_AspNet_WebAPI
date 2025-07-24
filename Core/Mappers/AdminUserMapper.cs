using AutoMapper;
using Core.Model.AdminUser;
using Domain.Data.Entities.Identity;

namespace Core.Mappers
{
    public class AdminUserMapper : Profile
    {
        public AdminUserMapper()
        {
            CreateMap<UserEntity, AdminUserItemModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.IsLoginGoogle, opt => opt.MapFrom(src => src.UserLogins!.Any(l => l.LoginProvider == "Google")))
                .ForMember(dest => dest.IsLoginPassword, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.PasswordHash)))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles!.Select(ur => ur.Role.Name).ToList()))
                .ForMember(dest => dest.LoginTypes, opt => opt.Ignore());
            CreateMap<AdminUserUpdateModel, UserEntity>()
                .ForMember(dest => dest.FirstName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.FirstName)))
                .ForMember(dest => dest.LastName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.LastName)))
                .ForMember(dest => dest.Email, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Email)));

        }
    }
}
