
using Microsoft.AspNetCore.Identity;
using Ntier.DAL.Context;
using Ntier.DAL.Entities;

namespace Ntier.DAL
{
    public static class Seeder
    {
        public static async Task SeedAsync(ShopContext context)
        {
            var roleId = Guid.NewGuid().ToString();
            if (!context.Roles.Any())
            {
                var roles = new AppRole
                {
                    Id = roleId,
                    Name = "admin",
                    NormalizedName = "ADMIN"
                };
                await context.AddAsync(roles);
                await context.SaveChangesAsync();
            }
            var userId = Guid.NewGuid().ToString();
            if (!context.Users.Any())
            {
                var hasherPassword = new PasswordHasher<AppUser>();

                var user = new AppUser()
                {
                    Id = userId,
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = false,
                };
                user.PasswordHash = hasherPassword.HashPassword(user, "Admin@123");
                await context.Users.AddAsync(user);
                await context.UserRoles.AddAsync(new IdentityUserRole<string>()
                {
                    RoleId = roleId,
                    UserId = userId,
                });
                await context.SaveChangesAsync();
            }

        }
    }
}
