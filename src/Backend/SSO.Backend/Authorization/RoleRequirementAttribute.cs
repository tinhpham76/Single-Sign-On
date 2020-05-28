using Microsoft.AspNetCore.Mvc;
using SSO.Backend.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
