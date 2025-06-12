using Domain.Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("tblCarts")]
    public class CartEntity
    {
        [Range(0, 50)]
        public int Quantity { get; set; }
        [ForeignKey("Product")]
        public long ProductId { get; set; }
        public virtual ProductEntity? Product { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
        public virtual UserEntity? User { get; set; }
    }
}
