using IdentityServer4.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SSO.Backend.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Authorization
{
    public class RoleRequirementFilter : IAuthorizationFilter
    {
        private readonly RoleCode _roleCode;
        public RoleRequirementFilter(RoleCode roleCode)
        {
            _roleCode = roleCode;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var roleClaim = context.HttpContext.User.Claims.SingleOrDefault(x => x.Type == "role");
            if(roleClaim != null)
            {
                var role = roleClaim.Value;
                if (!role.Equals(_roleCode.ToString()))
                {
                    context.Result = new JsonResult("No access, please contact the administrator!");
                }
            }
            else
            {
                context.Result = new JsonResult("No access, please contact the administrator!");
            }
        }
    }
}
