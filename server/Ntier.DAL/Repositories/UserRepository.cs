using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ntier.DAL.Entities;
using Ntier.DAL.Interfaces;
using Ntier.DAL.SeedWorks;

namespace Ntier.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        public UserRepository(UserManager<AppUser> userManager) 
        {
            _userManager = userManager;
        }
        public async Task<bool> AddUserAsync(AppUser user)
        {
            var oldUser = await _userManager.FindByEmailAsync(user.Email);
            if (oldUser != null)
            {
                return false;
            }
            await _userManager.CreateAsync(user, user.PasswordHash);
            await _userManager.AddToRoleAsync(user, RoleDefine.USER);
            return true;
        }

        public async Task<AppUser> GetByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        public async Task<AppUser?> GetUserByPasswordAsync(AppUser user)
        {
            var userFind = await _userManager.FindByEmailAsync(user.Email);
            var hashedPassword = new PasswordHasher<AppUser>();
            var result = hashedPassword.VerifyHashedPassword(userFind, userFind.PasswordHash, user.PasswordHash);
            if (result != PasswordVerificationResult.Success) 
            {
                return null;
            }
            return userFind;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task RemoveRefreshTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);  
            if (user != null)
            {
                user.RefreshTk = null;
                user.ExpireAt = null;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task<AppUser?> UpdateRefreshTokenAsync(string userId, string refreshToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.RefreshTk = refreshToken;
                user.ExpireAt = DateTime.UtcNow.AddHours(10);
                await _userManager.UpdateAsync(user);
            }
            return user;
        }
    }
}

