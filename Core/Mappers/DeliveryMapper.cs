using AutoMapper;
using Core.Model.General;
using Domain.Entities.Delivery;

namespace Core.Mappers
{
    public class DeliveryMapper : Profile
    {
        public DeliveryMapper()
        {
            CreateMap<CityEntity, SimpleModel>();
            CreateMap<PostDepartmentEntity, SimpleModel>();
            CreateMap<PaymentTypeEntity, SimpleModel>();
        }
    }
}
