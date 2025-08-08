using API.Data;
using API.Entities;
using API.Entities.Products;
using Bogus;
using Microsoft.AspNetCore.Identity;

namespace API.Seed
{
    public class RestoreCourseContextSeed
    {
        public static async Task SeedAsync(RestoreCourseDbContext context, UserManager<User> userManager)
        {
            await SeedUserManager(userManager);
            await SeedProductType(context);
            await SeedProductBrand(context);
            await SeedProduct(context);
        }
        
        private static async Task SeedUserManager(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    UserName = "developer",
                    Email = "developer@email.com"
                };
                
                await CreateUserAndRole(userManager, user, "Senha@123", new []{"Member"});

                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@email.com"
                };
                
                await CreateUserAndRole(userManager, user, "Senha@123", new []{"Admin", "Member"});

            }
        }

        private static async Task CreateUserAndRole(UserManager<User> userManager, User user, string pwd, string[] role)
        {
            await userManager.CreateAsync(user, pwd);
            await userManager.AddToRolesAsync(user, role);
        }

        private static async Task SeedProductType(RestoreCourseDbContext context)
        {
            if (!context.DbSet<ProductType>().Any())
            {
                var fakerProductType = new Faker<ProductType>()
                .RuleFor(x => x.Name, p => p.Commerce.ProductMaterial())
                .Generate(5);

                await context.AddRangeAsync(fakerProductType);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedProductBrand(RestoreCourseDbContext context)
        {
            if (!context.DbSet<ProductBrand>().Any())
            {
                var listLanguages = new string[]
                {
                    ".NET",
                    "Java",
                    "PHP",
                    "Redis",
                    "SQL Server",
                    "TypeScript",
                    "ReactJS",

                };
                var fakerProductBrand = new Faker<ProductBrand>()
                .RuleFor(x => x.Name, p => p.PickRandom(listLanguages))
                .Generate(5);

                await context.AddRangeAsync(fakerProductBrand);
                await context.SaveChangesAsync();
            }
        }

        private async static Task SeedProduct(RestoreCourseDbContext context)
        {
            if (!context.DbSet<Product>().Any())
            {
                var idProductBrands = context.DbSet<ProductBrand>().Select(x => x.Id).ToList();
                var idProductTypes = context.DbSet<ProductType>().Select(x => x.Id).ToList();

                var fakerProduct = new Faker<Product>()
                .RuleFor(x => x.Name, p => p.Commerce.Product())
                .RuleFor(x => x.Description, p => p.Commerce.ProductDescription())
                .RuleFor(x => x.PictureUrl, p => $"/images/products/File {p.Random.Int(1, 18)}.png")
                .RuleFor(x => x.Price, p => (long)p.Random.Decimal(5, 200))
                .RuleFor(x => x.TypeId, p => p.PickRandom(idProductTypes))
                .RuleFor(x => x.BrandId, p => p.PickRandom(idProductBrands))
                .RuleFor(x => x.QuantityInStock, p => p.Random.Int(1,150))
                .Generate(20);

                await context.AddRangeAsync(fakerProduct);
                await context.SaveChangesAsync();
            }
        }
    }
}