using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManagementSystem.Domain.Model;

namespace WarehouseManagementSystem.Infrastructure.Data;

public class RoleSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        var roles = new[] { "User", "Admin" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

        }

        var adminEmail = "admin@warehousemanagement.com";
        var adminPassword = "Admin123!";

        if (await userManager.FindByEmailAsync(adminEmail) is null)
        {
            var admin = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Akif",
                LastName = "Ismayilov",
                Address = "Sumgayit city",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            var result = await userManager.CreateAsync(admin, adminPassword);

            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }

    }
}