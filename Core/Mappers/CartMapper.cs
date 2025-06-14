using AutoMapper;
using Core.Model.Cart;
using Domain.Entities;

namespace Core.Mappers
{
    public class CartMapper : Profile
    {
        public CartMapper()
        {
            CreateMap<CartEntity, CartItemModel>()
                .ForMember(x => x.ProductId, opt => opt.MapFrom(x => x.Product!.Id))
                .ForMember(x => x.CategoryId, opt => opt.MapFrom(x => x.Product!.Category!.Id))
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Product!.Category!.Name))
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Product!.Name))
                .ForMember(x => x.Price, opt => opt.MapFrom(x => x.Product!.Price))
                .ForMember(x => x.ImageName, opt => opt.MapFrom(x =>
                    x.Product!.ProductImages != null && x.Product.ProductImages.Any()
                        ? x.Product.ProductImages.OrderBy(x => x.Prority).First().Name
                        : null));
            CreateMap<CartCreateUpdateModel, CartEntity>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}
