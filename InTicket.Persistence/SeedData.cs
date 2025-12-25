using InTicket.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace InTicket.Persistence;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = { "Admin", "User" };
        foreach (var role in roles)
        {
            var roleExist = await roleManager.RoleExistsAsync(role);
            if (!roleExist)
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var adminEmail = "admin@inticket.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var admin = new ApplicationUser
            {
                NationalId = "29912011234567",
                FirstName = "Ahmed",
                LastName = "Hassan",
                InTicketId = Guid.NewGuid(),
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                PhoneNumber = "+201012345678",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0 ,
                IsAdmin =  true
            };
            var result = await userManager.CreateAsync(admin, "Admin@123!");
            if(result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}