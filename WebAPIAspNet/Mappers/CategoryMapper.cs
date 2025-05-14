using AutoMapper;
using WebAPIAspNet.Data.Entities;
using WebAPIAspNet.Model.Category;
using WebAPIAspNet.Model.Seeder;

namespace WebAPIAspNet.Mappers
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<SeederCategoryModel, CategoryEntity>();
            CreateMap<CategoryEntity, CategoryItemModel>();
            CreateMap<CategoryCreateModel, CategoryEntity>()
                .ForMember(x => x.Image, opt => opt.Ignore());
        }
    }
}
