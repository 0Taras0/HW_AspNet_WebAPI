using AutoMapper;
using Core.Model.NovaPoshta.City;
using Core.Model.NovaPoshta.Department;
using Domain.Entities.Delivery;

namespace Core.Mappers
{
    public class NovaPoshtaMapper : Profile
    {
        public NovaPoshtaMapper()
        {
            CreateMap<CityItemResponse, CityEntity>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Description));

            CreateMap<DepartmentItemResponse, PostDepartmentEntity>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Description));
        }

    }
}
