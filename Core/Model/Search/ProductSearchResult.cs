using Core.Model.Search.Params;

namespace Core.Model.Search
{
    public class ProductSearchResult : SearchResult<ProductItemModel>
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
