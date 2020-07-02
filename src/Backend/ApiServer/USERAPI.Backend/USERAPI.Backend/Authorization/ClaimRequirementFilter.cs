using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USERAPI.Backend.Constants;

namespace USERAPI.Backend.Authorization
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        private readonly PermissionCode _permissionCode;
        public ClaimRequirementFilter(PermissionCode permissionCode)
        {
            _permissionCode = permissionCode;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var permissionsClaim = context.HttpContext.User.Claims
                .SingleOrDefault(c => c.Type == SystemConstants.Permission.Type);
            if (permissionsClaim != null)
            {
                var permissions = JsonConvert.DeserializeObject<List<string>>(permissionsClaim.Value);
                if (!permissions.Contains(_permissionCode.ToString()))
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
