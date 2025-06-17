using AutoMapper;
using Core.Model.Order;
using Domain.Entities;

namespace Core.Mappers
{
    public class OrderMapper : Profile
    {
        public OrderMapper()
        {
            CreateMap<OrderItemEntity, OrderItemModel>()
                .ForMember(x => x.ProductImage, opt => opt
                    .MapFrom(x => x.Product!.ProductImages!.OrderBy(x => x.Prority).First().Name))
                .ForMember(x => x.ProductName, opt => opt
                    .MapFrom(x => x.Product!.Name))
                .ForMember(x => x.ProductSlug, opt => opt
                    .MapFrom(x => x.Product!.Slug));

            CreateMap<OrderEntity, OrderModel>()
                .ForMember(x => x.Status, opt => opt.MapFrom(x => x.OrderStatus!.Name));
        }
    }
}
