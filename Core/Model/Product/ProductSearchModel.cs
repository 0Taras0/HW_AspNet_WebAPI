using System.ComponentModel.DataAnnotations;

namespace Core.Model.Product
{
    public class ProductSearchModel
    {
        public string? Name { get; set; }

        public string? Slug { get; set; }

        public int? CategoryId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Мінімальна ціна має бути більше або дорівнювати 0")]
        public decimal? MinPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Максимальна ціна має бути більше або дорівнювати 0")]
        public decimal? MaxPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Номер сторінки має бути >= 1")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Розмір сторінки має бути від 1 до 100")]
        public int ItemPerPage { get; set; } = 10;
    }
}
