namespace Core.Model.Category
{
    public class CategorySearchModel
    {
        public string Name { get; set; } = string.Empty;
        public string CategorySlug { get; set; } = string.Empty;
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
