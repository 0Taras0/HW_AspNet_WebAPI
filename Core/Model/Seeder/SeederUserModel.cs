using static Bogus.DataSets.Name;

namespace Core.Model.Seeder
{
    public class SeederUserModel
    {
        public Gender Gender { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }
}
