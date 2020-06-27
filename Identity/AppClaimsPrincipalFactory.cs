﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ObservrDeveloperTest.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ObservrDeveloperTest.Identity
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public AppClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager
            , IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
        { }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            if (!string.IsNullOrWhiteSpace(user.NewYorkBorough))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(ClaimTypes.StateOrProvince, user.NewYorkBorough)
                     });
            }

            return principal;
        }
    }
}
