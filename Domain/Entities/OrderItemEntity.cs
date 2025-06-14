using Domain.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class OrderItemEntity : BaseEntity<long>
    {
        public decimal PriceBuy { get; set; }
        public int Quantity { get; set; }
        [ForeignKey(nameof(Product))]
        public long ProductId { get; set; }
        [ForeignKey(nameof(Order))]
        public long OrderId { get; set; }
        public ProductEntity? Product { get; set; }
        public OrderEntity? Order { get; set; }
    }
}
