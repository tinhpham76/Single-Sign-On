using Microsoft.AspNetCore.Mvc;
using SSO.Backend.Constants;

namespace SSO.Backend.Authorization
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(PermissionCode permissionId)
            : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { permissionId };
        }
    }
}
