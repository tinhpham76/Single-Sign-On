using Microsoft.AspNetCore.Mvc;
using SSO.Backend.Constants;

namespace SSO.Backend.Authorization
{
    public class RoleRequirementAttribute : TypeFilterAttribute
    {
        public RoleRequirementAttribute(RoleCode roleId)
            : base(typeof(RoleRequirementFilter))
        {
            Arguments = new object[] { roleId };
        }
    }
}
