using AutoMapper;
using Core.Model.Seeder;
using Domain.Entities;

namespace Core.Mappers
{
    public class ProductSizeMapper : Profile
    {
        public ProductSizeMapper()
        {
            CreateMap<SeederProductSizeModel, ProductSizeEntity>();
        }
    }
}
