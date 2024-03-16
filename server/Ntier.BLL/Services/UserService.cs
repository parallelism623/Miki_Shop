using Microsoft.Extensions.Options;
using Ntier.BLL.Configuration;
using Ntier.BLL.Interfaces;
using Ntier.DAL.DTO.User;
using Ntier.DAL.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Ntier.DAL.Entities;
namespace Ntier.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;
        public UserService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings, IMapper mapper)
        {
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
            _userRepository = userRepository;
        }

        public async Task<string> GetNewAccessTokenAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user.RefreshTk != null)
            {
                if (user.ExpireAt < DateTime.Now)
                {
                    await _userRepository.RemoveRefreshTokenAsync(userId);
                    throw new ArgumentException("Refresh token was expired");
                }
                else
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim("UserId", user.Id),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())  
                    };
                    var token = await TokenServices.GenerateAccessToken(claims, _jwtSettings);
                    return token;
                }
            }
            else
            {
                throw new ArgumentException("UserId is invalid");
            }
        }

        public async Task<IEnumerable<UserRegisterDTO>> GetUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();
            if (users == null)
            {
                throw new Exception();
            }
            return _mapper.Map<IEnumerable<UserRegisterDTO>>(users);
        }

        public async Task<UserDTO> LoginUserAsync(UserLoginDTO userLoginDTO)
        {
            var user = await _userRepository.GetUserByPasswordAsync(_mapper.Map<AppUser>(userLoginDTO));
            if (user == null)
            {
                throw new Exception("Invalid user name or password");
            }
            var refreshToken = await TokenServices.GenerateRefreshToken();
            List<Claim> claims = new List<Claim>()
            {
                new Claim("UserId", user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var accessToken = await TokenServices.GenerateAccessToken(claims, _jwtSettings);
            var newUser = _mapper.Map<UserDTO>(await _userRepository.UpdateRefreshTokenAsync(user.Id, refreshToken));
            newUser.Access_token = accessToken;
            return newUser;
        }

        public async Task RegisterUserAsync(UserRegisterDTO userRegisterDTO)
        {
            var result = await _userRepository.AddUserAsync(_mapper.Map<AppUser>(userRegisterDTO));
            if (result == false)
            {
                throw new ArgumentException("Email already exits");
            }
        }
    }
}