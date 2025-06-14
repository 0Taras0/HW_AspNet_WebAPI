using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities
{
    [Table("tblOrderStatuses")]
    public class OrderStatusEntity : BaseEntity<long>
    {
        [StringLength(250)]
        public string Name { get; set; } = String.Empty;
    }
}
