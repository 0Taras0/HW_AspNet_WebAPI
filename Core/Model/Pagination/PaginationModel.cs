namespace Core.Model.Pagination
{
    public class PaginationModel<T> where T : class
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalPages { get; set; } = 0;
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
