using Domain.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("tblProducts")]
    public class ProductEntity : BaseEntity<long>
    {
        [StringLength(250)]
        public string Name { get; set; } = String.Empty;

        [StringLength(250)]
        public string Slug { get; set; } = String.Empty;

        public decimal Price { get; set; }

        public int Weight { get; set; }

        [ForeignKey("Category")]
        public long CategoryId { get; set; }

        public CategoryEntity? Category { get; set; }

        //Може бути розір може його не бути
        [ForeignKey("ProductSize")]
        public long? ProductSizeId { get; set; }

        public ProductSizeEntity? ProductSize { get; set; }
        public ICollection<ProductIngredientEntity>?  ProductIngredients { get; set; }
        public ICollection<ProductImageEntity>?  ProductImages { get; set; }
        public ICollection<CartEntity>? Carts { get; set; }
    }
}
