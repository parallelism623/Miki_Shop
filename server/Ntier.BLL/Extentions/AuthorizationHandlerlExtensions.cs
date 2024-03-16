using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using Ntier.DAL.Entities;
using Ntier.DAL.SeedWorks;
using System.Security.Claims;

namespace Ntier.BLL.Extentions
{
    public class AuthorizationHandlerlExtensions : AuthorizationHandler<RequirementAuthorizationExtensions>
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public AuthorizationHandlerlExtensions(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager) 
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RequirementAuthorizationExtensions requirement)
        {
            if (context.User.Identity.IsAuthenticated == false)
            {
                context.Fail();
                return;
            }
            var userId = (context.User.Identity as ClaimsPrincipal).FindFirst(x => x.Type == "UserId").Value;
            var listPermission = new List<string>();
            listPermission.Append(requirement._param);
            listPermission.AddRange(requirement._params);
            var user = await _userManager.FindByIdAsync(userId);
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
