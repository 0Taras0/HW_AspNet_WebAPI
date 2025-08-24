using Core.Model.Product;
using Core.Model.Product.Ingredient;
using Core.Model.Search;
using Core.Model.Search.Params;
using Domain.Entities;

namespace Core.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductItemModel>> List();
        Task<ProductSearchResult> SearchProductsAsync(ProductSearchModel model);
        Task<ProductItemModel> GetById(int id);
        Task<List<ProductItemModel>> GetBySlug(string slug);
        Task<ProductEntity> Create(ProductCreateModel model);
        Task<ProductItemModel> Edit(ProductEditModel model);

        Task<IEnumerable<ProductIngredientModel>> GetIngredientsAsync();
        Task<IEnumerable<ProductSizeModel>> GetSizesAsync();
        Task<ProductIngredientModel> UploadIngredient(CreateIngredientModel model);
        Task Delete(long id);
    }
}
