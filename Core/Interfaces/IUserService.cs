using Core.Model.AdminUser;
using Core.Model.Roles;
using Core.Model.Search;
using Core.Model.Search.Params;
using Core.Model.Seeder;

namespace Core.Interfaces
{
    public interface IUserService
    {
        Task<List<AdminUserItemModel>> GetAllUsersAsync();
        Task<SearchResult<AdminUserItemModel>> SearchUsersAsync(UserSearchModel model);
        Task<string> SeedUsersAsync(SeedItemsModel model);
        Task<AdminUserItemModel> GetUserByIdAsync(long id);
        Task<AdminUserItemModel> UpdateAsync(AdminUserUpdateModel model);
        Task<RolesItemModel> GetRolesAsync();
    }
}
