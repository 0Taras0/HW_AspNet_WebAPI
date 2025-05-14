using System.ComponentModel.DataAnnotations;

namespace WebAPIAspNet.Model.Category
{
    public class CategoryCreateModel
    {
        public string Name { get; set; } = String.Empty;
        public string Slug { get; set; } = String.Empty;
        public IFormFile Image { get; set; }
    }
}
