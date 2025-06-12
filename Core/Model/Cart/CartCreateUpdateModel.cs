namespace Core.Model.Cart
{
    public class CartCreateUpdateModel
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; } = 1;  
    }
}
