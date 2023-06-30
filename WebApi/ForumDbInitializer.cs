using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public class ForumDbInitializer
    {
        public static async Task InitializeAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, InitAdminRoleConfigurationData configurationData)
        {
            var adminEmail = configurationData.adminEmail;
            var adminPassword = configurationData.adminPassword;
            var adminNickName = configurationData.adminNickName;
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                ApplicationUser admin = new ApplicationUser { Email = adminEmail, UserName = adminEmail, NickName=adminNickName };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
