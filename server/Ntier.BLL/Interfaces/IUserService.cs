using Ntier.DAL.DTO.User;
using Ntier.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntier.BLL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserRegisterDTO>> GetUsersAsync();
        Task RegisterUserAsync(UserRegisterDTO userRegisterDTO);
        Task<UserDTO> LoginUserAsync(UserLoginDTO userLoginDTO);
        Task<string> GetNewAccessTokenAsync(string userId);
        Task LogoutAsync(string userId, string userName);
    }
}
