using AutoMapper;
using Core.Model.Category;
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
        }
    }
}
