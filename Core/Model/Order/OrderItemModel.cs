namespace Core.Model.Order
{
    public class OrderItemModel
    {
        public decimal PriceBuy { get; set; }
        public int Quantity { get; set; }
        public long ProductId { get; set; }
        public string ProductSlug { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;
    }
}
