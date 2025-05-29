using AutoMapper;
using Domain.Data.Entities.Identity;
using Domain.Data.Entities;
using Core.Model.Seeder;

namespace Core.Interfaces
{
    public class SeederMapper : Profile
    {
        public SeederMapper()
        {
            CreateMap<SeederUserModel, UserEntity>()
                .ForMember(x => x.Image, opt => opt.Ignore());
        }
    }
}
