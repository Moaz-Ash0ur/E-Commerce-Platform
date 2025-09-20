using Microsoft.AspNetCore.Identity;
using ShoppingDAL.Data;
using ShoppingDAL.Domains;

namespace OnlineShoppingApi.Services
{
    public class ContextConfig
    {
        private static readonly string seedAdminEmail = "admin@gmail.com";
        public static async Task SeedDataAsync(ShoppingDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await SeedUserAsync(userManager, roleManager);
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Ensure roles exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Ensure admin user exists
            var adminEmail = seedAdminEmail;
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var id = Guid.NewGuid().ToString();
                adminUser = new ApplicationUser
                {
                    Id = id,
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "admin",
                    LastName = "admin",
                    PhoneNumber = "01020429118"
                };
                var result = await userManager.CreateAsync(adminUser, "admin123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }


}


