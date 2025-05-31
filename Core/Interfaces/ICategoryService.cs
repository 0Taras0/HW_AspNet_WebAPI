using Core.Model.Category;

namespace Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryItemModel>> ListAsync();
        Task<CategoryItemModel?> GetItemByIdAsync(int id);
        Task<CategoryItemModel> CreateAsync(CategoryCreateModel model);
        Task<CategoryItemModel> UpdateAsync(CategoryUpdateModel model);
        Task<CategoryItemModel> DeleteAsync(long id);
    }
}
