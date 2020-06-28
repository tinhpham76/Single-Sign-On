using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using SSO.Backend.Data;
using SSO.Backend.Data.Entities;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SSO.Backend.Services
{
    public class IdentityProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityProfileService(IUserClaimsPrincipalFactory<User> claimsFactory,
            UserManager<User> userManager,
            ApplicationDbContext context,
           RoleManager<IdentityRole> roleManager)
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
            {
                throw new ArgumentException("");
            }

            var principal = await _claimsFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();
            var roles = await _userManager.GetRolesAsync(user);          


            //Add more claims like this
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim("fullName", user.LastName + " " + user.FirstName));
            claims.Add(new Claim(ClaimTypes.Role, string.Join(";", roles)));



            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
