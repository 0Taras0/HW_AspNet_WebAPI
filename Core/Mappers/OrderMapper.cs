﻿using AutoMapper;
using Core.Model.Delivery;
using Core.Model.Order;
using Domain.Entities;
using Domain.Entities.Delivery;

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

            CreateMap<CartEntity, OrderItemEntity>()
            .ForMember(x => x.PriceBuy, opt => opt
            .MapFrom(x => x.Product!.Price))
            .ForMember(x => x.Quantity, opt => opt
            .MapFrom(x => x.Quantity));

            CreateMap<DeliveryInfoCreateModel, DeliveryInfoEntity>();
        }
    }
}
