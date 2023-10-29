using FlashCardDemo.Enums;
using FlashCardDemo.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FlashCardDemo.DAL
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<FlashCardDemoUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // seeding Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
        }
    }
}