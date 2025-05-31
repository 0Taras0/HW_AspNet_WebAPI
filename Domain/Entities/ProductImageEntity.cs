using Domain.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("tblProductImages")]
    public class ProductImageEntity : BaseEntity<long>
    {
        [StringLength(250)]
        public string Name { get; set; } = String.Empty;

        public short Prority { get; set; }

        [ForeignKey("Product")]
        public long ProductId { get; set; }

        public ProductEntity? Product { get; set; }
    }
}
