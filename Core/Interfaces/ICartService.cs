using Core.Model.Cart;

namespace Core.Interfaces
{
    public interface ICartService
    {
        Task CreateUpdateAsync(CartCreateUpdateModel model);
        Task<List<CartItemModel>> GetCartItemsAsync();
        Task DeleteAsync(long id);
    }
}
