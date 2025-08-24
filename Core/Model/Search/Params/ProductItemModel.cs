using Core.Model.Category;
using Core.Model.Product;

namespace Core.Model.Search.Params
{
    public class ProductItemModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Weight { get; set; }
        public CategoryItemModel? Category { get; set; }
        public ProductSizeModel? ProductSize { get; set; }
        public List<ProductIngredientModel>? ProductIngredients { get; set; }
        public List<ProductImageModel>? ProductImages { get; set; }
    }
}
