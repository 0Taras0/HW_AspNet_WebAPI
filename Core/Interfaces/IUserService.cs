using Core.Model.AdminUser;

namespace Core.Interfaces
{
    public interface IUserService
    {
        Task<List<AdminUserItemModel>> GetAllUsersAsync();
    }
}
