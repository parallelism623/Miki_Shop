using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Ntier.DAL.Entities;
using Ntier.DAL.Interfaces;
using Ntier.DAL.SeedWorks;
using System.Security.Claims;

namespace Ntier.BLL.Extentions
{
    public class AuthorizationHandlerlExtensions : AuthorizationHandler<RequirementAuthorizationExtensions>
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICacheRepository _cacheRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthorizationHandlerlExtensions(ICacheRepository cacheRepository, IHttpContextAccessor httpContextAccessor , RoleManager<AppRole> roleManager, UserManager<AppUser> userManager) 
        {
            _httpContextAccessor = httpContextAccessor;
            _cacheRepository = cacheRepository;
            _roleManager = roleManager;
            _userManager = userManager;
            //_httpContextAccessor = httpContextAccessor; 
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RequirementAuthorizationExtensions requirement)
        {
            if (context.User.Identity.IsAuthenticated == false)
            {
                context.Fail();
                return;
            }
            //var accessToken = _httpContextAccessor.HttpContext.Request.Headers["access_token"];

            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["access_token"];
            var userId = (context.User.Identity as ClaimsPrincipal).FindFirst(x => x.Type == "UserId").Value;
            var user = await _userManager.FindByIdAsync(userId);
            var accessTokenInCache = await _cacheRepository.GetCacheResponeAsync(user.UserName);
            if (accessTokenInCache is null || accessToken.Equals(accessTokenInCache) == false)
            {
                context.Fail();
                return;
            }
            var listPermission = new List<string>();
            listPermission.Append(requirement._param);
            listPermission.AddRange(requirement._params);
            
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(RoleDefine.ADMIN))
            {
                context.Succeed(requirement);
                return;
            }
            if (listPermission.Any())
            {
                var allPermission = new List<Claim>();
                foreach (var role in roles)
                {
                    var nameOfRole = await _roleManager.FindByNameAsync(role);
                    var permissionOfRole = await _roleManager.GetClaimsAsync(nameOfRole);
                    allPermission.AddRange(permissionOfRole);
                }
                var resultPermission = allPermission.Select(x => x.Value).Intersect(listPermission).ToList();
                if (resultPermission.Count() != listPermission.Count())
                {
                    context.Fail();
                    return;
                }
            }
            context.Succeed(requirement);
            return;
        }
    }
}
