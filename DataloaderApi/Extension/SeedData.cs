using DataloaderApi.Data;
using Microsoft.AspNetCore.Identity;

namespace IdentityAuthTest.Services
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {


            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "User"};


            foreach (var roleName in roleNames)
            {


                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }


            var admin = await userManager.GetUsersInRoleAsync("Admin");

            var adminuser = new ApplicationUser
            {

                Email = "sanyikafasza@kkkk.com",
                UserName = "Sanyi"

            };

            string initailPassword = "Password1234*";

            var result = await userManager.CreateAsync(adminuser, initailPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminuser, "Admin");

            }
        }
    }
}
