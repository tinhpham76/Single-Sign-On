using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USERAPI.Backend.Constants;

namespace USERAPI.Backend.Authorization
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
