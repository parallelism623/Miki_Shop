using Microsoft.IdentityModel.Tokens;
using Ntier.BLL.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Ntier.BLL.Services
{
    public static class TokenServices
    {
        
        public static async Task<string> GenerateAccessToken(IEnumerable<Claim> claims, JwtSettings jwtSettings)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var authenKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescription = new JwtSecurityToken
            (
               claims: claims ,
               expires: DateTime.UtcNow.AddMinutes(1),
               signingCredentials: authenKey
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenDescription);
            return tokenString;
        }
        public static async Task<string?> GenerateRefreshToken()
        {
            var refresh = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(refresh);
                return Convert.ToBase64String(refresh);
            };
        }
        public static async Task<ClaimsPrincipal?> GetClaimsPrincipal(string token, JwtSettings jwtSettings)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            };
            var tokenHanlder = new JwtSecurityTokenHandler();
            var principal = tokenHanlder.ValidateToken(token, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                throw new Exception("Invalid Token");
            }
            return principal;
        }
    }
}
