using Core.Model.Product;
using Domain.Entities;

namespace Core.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductItemModel>> List();
        Task<ProductItemModel> GetById(int id);
        Task<List<ProductItemModel>> GetBySlug(string slug);
        Task<ProductEntity> Create(ProductCreateModel model);

        public Task<IEnumerable<ProductIngredientModel>> GetIngredientsAsync();
        public Task<IEnumerable<ProductSizeModel>> GetSizesAsync();
    }
}
