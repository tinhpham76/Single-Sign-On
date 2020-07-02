using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USERAPI.Backend.Authorization;
using USERAPI.Backend.Constants;
using USERAPI.Backend.Models;
using USERAPI.Backend.Services;

namespace USERAPI.Backend.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserApiClient _userApiClient;

        public UsersController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        [HttpGet("{id}")]
        [ClaimRequirement(PermissionCode.USER_VIEW)]
        public async Task<ActionResult> GetUserDetail(string id)
        {
            var user = await _userApiClient.GetById(id);
            return Ok(user);
        }

        [HttpPut("{id}")]
        [ClaimRequirement(PermissionCode.USER_UPDATE)]
        public async Task<ActionResult> PutUser(string id, [FromBody]UserUpdateRequest request)
        { 
            var result = await _userApiClient.PutUser(id, request);
            if (result == true)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("{id}/change-password")]
        [ClaimRequirement(PermissionCode.USER_UPDATE)]
        public async Task<IActionResult> ChangePassword(string id, [FromBody]UserPasswordChangeRequest request)
        {
            var result = await _userApiClient.ChangePassword(id, request);
            if (result == true)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        [ClaimRequirement(PermissionCode.USER_DELETE)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userApiClient.DeleteUser(id);
            if (result == true)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
