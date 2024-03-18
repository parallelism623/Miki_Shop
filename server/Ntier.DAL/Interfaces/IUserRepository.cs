using Ntier.DAL.DTO.User;
using Ntier.DAL.Entities;
namespace Ntier.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetByIdAsync(string userId);
        Task RemoveRefreshTokenAsync(string userId);
        Task<AppUser> UpdateRefreshTokenAsync(string userId, string refreshToken);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByPasswordAsync(AppUser user);
        Task<bool> AddUserAsync(AppUser user);
        
    }
}
