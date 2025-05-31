using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Domain.Constants;
using Domain.Data;
using Domain.Data.Entities;
using Domain.Data.Entities.Identity;
using Core.Interfaces;
using Core.Model.Seeder;
using Core.Services;
using Domain.Entities;

namespace Domain
{
    public static class DbSeeder
    {
        public static async Task SeedData(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();
            //Цей об'єкт буде верта посилання на конткетс, який зараєстрвоано в Progran.cs
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();

            context.Database.Migrate();

            if (!context.Categories.Any())
            {
                var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Categories.json");
                if (File.Exists(jsonFile))
                {
                    var jsonData = await File.ReadAllTextAsync(jsonFile);
                    try
                    {
                        var categories = JsonSerializer.Deserialize<List<SeederCategoryModel>>(jsonData);
                        var entityItems = mapper.Map<List<CategoryEntity>>(categories);
                        foreach (var entity in entityItems)
                        {
                            entity.Image =
                                await imageService.SaveImageFromUrlAsync(entity.Image);
                        }

                        await context.Categories.AddRangeAsync(entityItems);
                        await context.SaveChangesAsync();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Not Found File Categories.json");
                }
            }

            if (!context.Ingredients.Any())
            {
                var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Ingredients.json");
                if (File.Exists(jsonFile))
                {
                    var jsonData = await File.ReadAllTextAsync(jsonFile);
                    try
                    {
                        var ingredients = JsonSerializer.Deserialize<List<SeederIngredientModel>>(jsonData);
                        var entityItems = mapper.Map<List<IngredientEntity>>(ingredients);
                        foreach (var entity in entityItems)
                        {
                            entity.Image =
                                await imageService.SaveImageFromUrlAsync(entity.Image);
                        }

                        await context.Ingredients.AddRangeAsync(entityItems);
                        await context.SaveChangesAsync();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Not Found File Categories.json");
                }
            }

            if (!context.ProductSizes.Any())
            {
                var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "ProductSizes.json");
                if (File.Exists(jsonFile))
                {
                    var jsonData = await File.ReadAllTextAsync(jsonFile);
                    try
                    {
                        var productSizes = JsonSerializer.Deserialize<List<SeederProductSizeModel>>(jsonData);
                        var entityItems = mapper.Map<List<ProductSizeEntity>>(productSizes);

                        await context.ProductSizes.AddRangeAsync(entityItems);
                        await context.SaveChangesAsync();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Not Found File Categories.json");
                }
            }


            if (!context.Products.Any())
            {
                var сaesar = new ProductEntity
                {
                    Name = "Цезаре",
                    Slug = "caesar",
                    Price = 195,
                    Weight = 540,
                    CategoryId = 7, // Assuming the first category is for Caesar
                    ProductSizeId = 1 // Assuming the first size is for Caesar
                };

                context.Products.Add(сaesar);
                await context.SaveChangesAsync();

                var ingredients = context.Ingredients.ToList();

                foreach (var ingredient in ingredients)
                {
                    var productIngredient = new ProductIngredientEntity
                    {
                        ProductId = сaesar.Id,
                        IngredientId = ingredient.Id
                    };
                    context.ProductIngredients.Add(productIngredient);
                }
                await context.SaveChangesAsync();

                string[] images = {
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQRN9gItVjEVGS7l2_WkYpNfWJa5y_XQcZ0hQ&s",
                "https://cdn.lifehacker.ru/wp-content/uploads/2022/03/11187_1522960128.7729_1646727034-1170x585.jpg",
                "https://i.obozrevatel.com/food/recipemain/2020/2/5/zhenygohvrxm865gbgzsoxnru3mxjfhwwjd4bmvp.jpeg?size=636x424"
            };
                foreach (var imageUrl in images)
                {
                    try
                    {
                        var productImage = new ProductImageEntity
                        {
                            ProductId = сaesar.Id,
                            Name = await imageService.SaveImageFromUrlAsync(imageUrl)
                        };
                        context.ProductImages.Add(productImage);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Save Image {0} - {1}", imageUrl, ex.Message);
                    }

                }
                await context.SaveChangesAsync();

            }

            if (!context.Roles.Any())
            {
                foreach (var role in Roles.AllRoles)
                {
                    var result = await roleManager.CreateAsync(new(role));
                    if (!result.Succeeded)
                    {
                        Console.WriteLine("Error Create Role {0}", role);
                    }
                }
            }

            await SeedUsers(context, mapper, userManager, imageService);
        }
        private async static Task<IFormFile> LoadImageAsFormFileAsync(string imagePath, string imageName)
        {
            var fileInfo = new FileInfo(imagePath);

            if (!File.Exists(imagePath))
            {
                return null;
            }

            var memoryStream = new MemoryStream(await File.ReadAllBytesAsync(imagePath));

            return new FormFile(memoryStream, 0, memoryStream.Length, "ImageFile", imageName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/" + fileInfo.Extension.Trim('.')
            };
        }

        private async static Task SeedUsers(AppDbContext context, IMapper mapper, UserManager<UserEntity> userManager, IImageService imageService)
        {
            if (!context.Users.Any())
            {
                await SeedAmin(context, userManager);
                await SeedUsersFromJson(context, mapper, userManager, imageService);
            }
        }

        private async static Task SeedUsersFromJson(AppDbContext context, IMapper mapper, UserManager<UserEntity> userManager, IImageService imageService)
        {
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Users.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var users = JsonSerializer.Deserialize<List<SeederUserModel>>(jsonData);
                    foreach (var model in users)
                    {
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "SeedImages", "Users", model.Image);
                        var formFile = await LoadImageAsFormFileAsync(imagePath, model.Image);

                        if (formFile == null)
                        {
                            Console.WriteLine($"Image file not found: {model.Image}");
                            continue;
                        }

                        var entity = mapper.Map<UserEntity>(model);
                        entity.Image = await imageService.SaveImageAsync(formFile);
                        var result = await userManager.CreateAsync(entity, model.Password);

                        if (result.Succeeded)
                        {
                            Console.WriteLine($"Користувача успішно створено {entity.LastName} {entity.FirstName}!");
                            await userManager.AddToRoleAsync(entity, Roles.User);
                        }
                        else
                        {
                            Console.WriteLine($"Помилка створення користувача:");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine($"- {error.Code}: {error.Description}");
                            }
                        }
                    }
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File Users.json");
            }
        }
        private async static Task SeedAmin(AppDbContext context, UserManager<UserEntity> userManager)
        {
            string email = "admin@gmail.com";
            var user = new UserEntity
            {
                UserName = email,
                Email = email,
                FirstName = "Адмін",
                LastName = "Батькович"
            };

            var result = await userManager.CreateAsync(user, "123456");
            if (result.Succeeded)
            {
                Console.WriteLine($"Користувача успішно створено {user.LastName} {user.FirstName}!");
                await userManager.AddToRoleAsync(user, Roles.Admin);
            }
            else
            {
                Console.WriteLine($"Помилка створення користувача:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Code}: {error.Description}");
                }
            }
        }
    }
}
