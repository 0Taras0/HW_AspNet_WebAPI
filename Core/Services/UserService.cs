using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bogus;
using Core.Interfaces;
using Core.Model.AdminUser;
using Core.Model.Roles;
using Core.Model.Search;
using Core.Model.Search.Params;
using Core.Model.Seeder;
using Domain.Constants;
using Domain.Data;
using Domain.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static Bogus.DataSets.Name;


namespace Core.Services
{
    public class UserService(UserManager<UserEntity> userManager,
    IMapper mapper, IImageService imageService, RoleManager<RoleEntity> roleManager, AppDbContext context) : IUserService
    {
        public async Task<List<AdminUserItemModel>> GetAllUsersAsync()
        {
            var users = await userManager.Users
                .ProjectTo<AdminUserItemModel>(mapper.ConfigurationProvider)
                .ToListAsync();

            await context.UserLogins.ForEachAsync(login =>
            {
                var user = users.FirstOrDefault(u => u.Id == login.UserId);
                if (user != null)
                {
                    user.LoginTypes.Add(login.LoginProvider);
                }
            });

            await context.Users.ForEachAsync(user =>
            {
                var adminUser = users.FirstOrDefault(u => u.Id == user.Id);
                if (adminUser != null)
                {
                    if (!string.IsNullOrEmpty(user.PasswordHash))
                    {
                        adminUser.LoginTypes.Add("Password");
                    }
                }
            });

            return users;
        }

        public async Task<RolesItemModel> GetRolesAsync()
        {
            var roles = await Task.FromResult(roleManager.Roles
                .Select(r => r.Name)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct()
                .ToList());

            return  new RolesItemModel
            {
                Roles = roles
            };
        }

        public async Task<AdminUserItemModel> GetUserByIdAsync(long id)
        {
            var userEntity = await context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserLogins)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (userEntity == null)
            {
                return null;
            }

            var userModel = mapper.Map<AdminUserItemModel>(userEntity);

            var loginProviders = userEntity.UserLogins?.Select(l => l.LoginProvider).Distinct().ToList();
            if (loginProviders != null)
                userModel.LoginTypes.AddRange(loginProviders);

            if (userEntity.PasswordHash != null)
                userModel.LoginTypes.Add("Password");

            return userModel;
        }

        public async Task<SearchResult<AdminUserItemModel>> SearchUsersAsync(UserSearchModel model)
        {
            var query = userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                string nameFilter = model.Name.Trim().ToLower().Normalize();

                query = query.Where(u =>
                    (u.FirstName + " " + u.LastName).ToLower().Contains(nameFilter) ||
                    u.FirstName.ToLower().Contains(nameFilter) ||
                    u.LastName.ToLower().Contains(nameFilter));
            }

            if (model?.StartDate != null)
            {
                query = query.Where(u => u.DateCreated >= model.StartDate);
            }

            if (model?.EndDate != null)
            {
                query = query.Where(u => u.DateCreated <= model.EndDate);
            }

            if (model.Roles != null && model.Roles.Any())
            {
                query = query.Where(user => model.Roles.Any(role => user.UserRoles.Select(x => x.Role.Name).Contains(role)));
            }

            var totalCount = await query.CountAsync();

            var safeItemsPerPage = model.ItemPerPage < 1 ? 10 : model.ItemPerPage;
            var totalPages = (int)Math.Ceiling(totalCount / (double)safeItemsPerPage);
            var safePage = Math.Min(Math.Max(1, model.Page), Math.Max(1, totalPages));

            var users = await query
                .OrderBy(u => u.Id)
                .Skip((safePage - 1) * safeItemsPerPage)
                .Take(safeItemsPerPage)
                .ProjectTo<AdminUserItemModel>(mapper.ConfigurationProvider)
                .ToListAsync();

            var userIds = users.Select(u => u.Id).ToList();

            var logins = await context.UserLogins
                .Where(l => userIds.Contains(l.UserId))
                .ToListAsync();

            var passwordUsers = await context.Users
                .Where(u => userIds.Contains(u.Id) && u.PasswordHash != null)
                .Select(u => u.Id)
                .ToListAsync();

            foreach (var user in users)
            {
                var userLogins = logins.Where(l => l.UserId == user.Id).Select(l => l.LoginProvider).Distinct();
                user.LoginTypes.AddRange(userLogins);

                if (passwordUsers.Contains(user.Id))
                {
                    user.LoginTypes.Add("Password");
                }
            }

            return new SearchResult<AdminUserItemModel>
            {
                Items = users,
                Pagination = new PaginationModel
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    ItemsPerPage = safeItemsPerPage,
                    CurrentPage = safePage
                }
            };
        }

        public async Task<string> SeedUsersAsync(SeedItemsModel model)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var fakeUsers = new Faker<SeederUserModel>("uk")
                .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
               //Pick some fruit from a basket
               .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender))
               .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender))
               .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
               .RuleFor(u => u.Password, (f, u) => f.Internet.Password(8))
               .RuleFor(u => u.Roles, f => new List<string>() { f.PickRandom(Roles.AllRoles) })
               .RuleFor(u => u.Image, f => "https://thispersondoesnotexist.com");

            var genUsers = fakeUsers.Generate(model.Count);

            try
            {
                foreach (var user in genUsers)
                {
                    var entity = mapper.Map<UserEntity>(user);
                    entity.UserName = user.Email;
                    entity.Image = await imageService.SaveImageFromUrlAsync(user.Image);
                    var result = await userManager.CreateAsync(entity, user.Password);
                    if (!result.Succeeded)
                    {
                        Console.WriteLine("Error Create User {0}", user.Email);
                        continue;
                    }
                    foreach (var role in user.Roles)
                    {
                        if (await roleManager.RoleExistsAsync(role))
                        {
                            await userManager.AddToRoleAsync(entity, role);
                        }
                        else
                        {
                            Console.WriteLine("Not Found Role {0}", role);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Json Parse Data {0}", ex.Message);
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            return elapsedTime;
        }

        public async Task<AdminUserItemModel> UpdateAsync(AdminUserUpdateModel model)
        {
            var userEntity = await context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserLogins)
                .FirstOrDefaultAsync(u => u.Id == model.Id);

            userEntity = mapper.Map(model, userEntity);

            if (model.ImageFile != null)
            {
                await imageService.DeleteImageAsync(userEntity.Image);
                userEntity.Image = await imageService.SaveImageAsync(model.ImageFile);
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                await userManager.ResetPasswordAsync(userEntity, 
                    await userManager.GeneratePasswordResetTokenAsync(userEntity), model.Password);
            }

            if (model.Roles.Count > 0)
            {
                await userManager.RemoveFromRolesAsync(userEntity, userEntity.UserRoles.Select(ur => ur.Role.Name));
                foreach (var role in model.Roles)
                {
                    if (await context.Roles.AnyAsync(r => r.Name == role))
                    {
                        await userManager.AddToRoleAsync(userEntity, role);
                    }
                    else
                    {
                        throw new Exception($"Role '{role}' does not exist.");
                    }
                }
            }

            await context.SaveChangesAsync();

            AdminUserItemModel updatedUser = mapper.Map<AdminUserItemModel>(userEntity);

            var loginProviders = userEntity.UserLogins?.Select(l => l.LoginProvider).Distinct().ToList();
            if (loginProviders != null)
                updatedUser.LoginTypes.AddRange(loginProviders);

            if (userEntity.PasswordHash != null)
                updatedUser.LoginTypes.Add("Password");

            return updatedUser;
        }

        private async Task LoadLoginsAndRolesAsync(List<AdminUserItemModel> users)
        {
            await context.UserLogins.ForEachAsync(login =>
            {
                var user = users.FirstOrDefault(u => u.Id == login.UserId);
                if (user != null)
                {
                    user.LoginTypes.Add(login.LoginProvider);
                }
            });

            var identityUsers = await userManager.Users.AsNoTracking().ToListAsync();

            foreach (var identityUser in identityUsers) // Забрав foreachAsync через конфлікнт з userManager.GetRolesAsync(identityUser)
            {
                var adminUser = users.FirstOrDefault(u => u.Id == identityUser.Id);
                if (adminUser != null)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);
                    adminUser.Roles = roles.ToList();

                    if (!string.IsNullOrEmpty(identityUser.PasswordHash))
                    {
                        adminUser.LoginTypes.Add("Password");
                    }
                }
            }
        }
    }
}
