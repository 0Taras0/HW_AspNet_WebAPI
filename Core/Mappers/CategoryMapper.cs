using AutoMapper;
using Domain.Data.Entities;
using Core.Model.Category;
using Core.Model.Seeder;

namespace Core.Interfaces
{   
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<SeederCategoryModel, CategoryEntity>();

            CreateMap<CategoryEntity, CategoryItemModel>();

            CreateMap<CategoryCreateModel, CategoryEntity>()
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name.Trim()))
                .ForMember(x => x.Slug, opt => opt.MapFrom(x => x.Slug.Trim()))
                .ForMember(x => x.Image, opt => opt.Ignore());

            CreateMap<CategoryUpdateModel, CategoryEntity>()
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name.Trim()))
                .ForMember(x => x.Slug, opt => opt.MapFrom(x => x.Slug.Trim()))
                .ForMember(x => x.Image, opt => opt.Ignore());
        }
    }
}
