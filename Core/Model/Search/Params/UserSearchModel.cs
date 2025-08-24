namespace Core.Model.Search.Params
{
    public class UserSearchModel
    {
        public List<string>? Roles { get; set; }
        public string? Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Page { get; set; } = 1;
        public int ItemPerPage { get; set; } = 10;
    }
}
