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
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.LoginTypes, opt => opt.Ignore());
        }
    }
}
