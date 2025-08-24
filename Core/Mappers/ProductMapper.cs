using AutoMapper;
using Core.Model.Product;
using Core.Model.Search.Params;
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
            CreateMap<ProductCreateModel, ProductEntity>()
                .ForMember(x => x.ProductImages, opt => opt.Ignore())
                .ForMember(x => x.ProductIngredients, opt => opt.Ignore());
            CreateMap<ProductEditModel, ProductEntity>()
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.ProductSize, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
            .ForMember(dest => dest.ProductIngredients, opt => opt.Ignore());
        }
    }
}
