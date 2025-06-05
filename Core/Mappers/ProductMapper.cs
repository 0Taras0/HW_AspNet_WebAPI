using AutoMapper;
using Core.Model.Product;
using Domain.Entities;

namespace Core.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<ProductImageEntity, ProductImageModel>();
            CreateMap<ProductEntity, ProductItemModel>()
                .ForMember(src => src.ProductImages, opt => opt
                    .MapFrom(x => x.ProductImages!.OrderBy(x => x.Prority)))
                .ForMember(src => src.ProductIngredients, opt => opt
                    .MapFrom(x => x.ProductIngredients!.Select(x => x.Ingredient)));
        }
    }
}
