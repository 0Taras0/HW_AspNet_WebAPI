using AutoMapper;
using Core.Model.Category;
using Core.Model.Product;
using Core.Model.Product.Ingredient;
using Core.Model.Seeder;
using Domain.Data.Entities;
using Domain.Entities;

namespace Core.Mappers
{
    public class IngredientMapper : Profile
    {
        public IngredientMapper()
        {
            CreateMap<SeederIngredientModel, IngredientEntity>();
            CreateMap<IngredientEntity, ProductIngredientModel>();
            CreateMap<CreateIngredientModel, IngredientEntity>()
                .ForMember(x => x.Image, opt => opt.Ignore());
        }
    }
}
