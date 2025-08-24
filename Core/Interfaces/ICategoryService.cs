using Core.Model.Category;
using Core.Model.Pagination;

namespace Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryItemModel>> ListAsync();
        Task<CategoryItemModel?> GetItemByIdAsync(int id);
        Task<string> GetCategoryNameById(int id);
        Task<CategoryItemModel> CreateAsync(CategoryCreateModel model);
        Task<CategoryItemModel> UpdateAsync(CategoryUpdateModel model);
        Task<CategoryItemModel> DeleteAsync(long id);
        Task<PaginationModel<CategoryItemModel>> ListAsync(CategorySearchModel searchModel);
    }
}
