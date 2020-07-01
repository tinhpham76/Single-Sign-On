using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Authorization;
using SSO.Backend.Constants;
using SSO.Backend.Data.Entities;
using SSO.Services;
using SSO.Services.RequestModel.User;
using SSO.Services.ViewModel.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Users
{
    public class UsersController : BaseController
    {
        #region User
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        //Post new user
        [HttpPost]
        [ClaimRequirement(PermissionCode.SSO_CREATE)]
        public async Task<IActionResult> PostUser([FromBody] UserCreateRequest request)
        {
            var user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Dob = DateTime.Parse(request.Dob),
                CreateDate = DateTime.UtcNow

            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, request);
            }
            return BadRequest();
        }

        //Get all user info
        [HttpGet]
        [ClaimRequirement(PermissionCode.SSO_VIEW)]
        public async Task<IActionResult> GetUsers()
        {
            var user = User.Identity.Name;
            var users = await _userManager.Users.Select(x => new UserViewModel()
            {
                Id = x.Id,
                UserName = x.UserName,
                FirstName = x.FirstName,
                LastName = x.FirstName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Dob = x.Dob,
                CreateDate = x.CreateDate
            }).ToListAsync();
            return Ok(users);
        }

        //Find user with User Name, Email, First Name, Last Name, Phone Number 
        [HttpGet("filter")]
        [ClaimRequirement(PermissionCode.SSO_VIEW)]
        public async Task<IActionResult> GetUsersPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Email.Contains(filter)
                || x.UserName.Contains(filter)
                || x.PhoneNumber.Contains(filter)
                || x.FirstName.Contains(filter)
                || x.LastName.Contains(filter));
            }
            var totalReconds = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new UserViewModel()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    FullName = x.LastName + ' ' + x.FirstName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Dob = x.Dob,
                    CreateDate = x.CreateDate
                }).ToListAsync();

            var pagination = new Pagination<UserViewModel>
            {
                Items = items,
                TotalRecords = totalReconds
            };
            return Ok(pagination);
        }

        //Get detail user with user id
        [HttpGet("{id}")]
        [ClaimRequirement(PermissionCode.SSO_VIEW)]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userViewModel = new UserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Dob = user.Dob,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreateDate = user.CreateDate
            };
            return Ok(userViewModel);
        }

        //Put user wiht user id
        [HttpPut("{id}")]
        [ClaimRequirement(PermissionCode.SSO_UPDATE)]
        public async Task<IActionResult> PutUser(string id, [FromBody] UserCreateRequest request)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Dob = DateTime.Parse(request.Dob);
            user.PhoneNumber = request.PhoneNumber;
            user.LastModifiedDate = DateTime.Now;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
        }


        //Put reset user password with user id
        [HttpPut("{id}/reset-password")]
        [ClaimRequirement(PermissionCode.SSO_UPDATE)]
        public async Task<IActionResult> PutResetPassword(string id)
        {

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound($"Cannot found user with id: {id}");
            var newPassword = _userManager.PasswordHasher.HashPassword(user, "User@123");
            user.PasswordHash = newPassword;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest();
        }

        //Put user password with user id
        [HttpPut("{id}/change-password")]
        [ClaimRequirement(PermissionCode.SSO_UPDATE)]
        public async Task<IActionResult> PutUserPassword(string id, [FromBody] UserPasswordChangeUpdateRequest request)
        {

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound($"Cannot found user with id: {id}");


            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest();
        }

        //Delete user with user id
        [HttpDelete("{id}")]
        [ClaimRequirement(PermissionCode.SSO_DELETE)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var adminUsers = await _userManager.GetUsersInRoleAsync(SystemConstants.Roles.Admin);
            var otherUsers = adminUsers.Where(x => x.Id != id).ToList();
            if (otherUsers.Count == 0)
            {
                return BadRequest("You cannot remove the only admin user remaining.");
            }
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                var uservm = new UserViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Dob = user.Dob,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreateDate = user.CreateDate
                };
                return Ok(uservm);
            }
            return BadRequest();

        }

        [HttpGet("{userId}/roles")]
        [ClaimRequirement(PermissionCode.SSO_VIEW)]
        public async Task<IActionResult> GetUserRoles(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Count == 0)
            {
                var dataRole = await _roleManager.Roles.Select(x => new UserRoleViewModel()
                {
                    Label = x.Id,
                    Value = x.Id,
                    Checked = userRoles.Contains(x.Id) ? true : false,
                    Disabled = false,
                    Name = x.Name
                }).ToListAsync();
                return Ok(dataRole);
            }
            var dataRoles = await _roleManager.Roles.Select(x => new UserRoleViewModel()
            {
                Label = x.Id,
                Value = x.Id,
                Checked = userRoles.Contains(x.Id) ? true : false,
                Disabled = userRoles.Contains(x.Id) ? false : true,
                Name = x.Name
            }).ToListAsync();
            return Ok(dataRoles);
        }

        [HttpPost("{userId}/roles")]
        [ClaimRequirement(PermissionCode.SSO_CREATE)]
        public async Task<IActionResult> PostRolesToUser(string userId, [FromBody] RoleAssignRequest request)
        {
            if (request.RoleNames?.Length == 0)
            {
                return BadRequest("Role names cannot empty");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"Cannot found user with id: {userId}");
            var result = await _userManager.AddToRolesAsync(user, request.RoleNames);
            if (result.Succeeded)
                return Ok();

            return BadRequest(result);
        }

        [HttpDelete("{userId}/roles")]
        [ClaimRequirement(PermissionCode.SSO_DELETE)]
        public async Task<IActionResult> RemoveRolesFromUser(string userId, [FromQuery] RoleAssignRequest request)
        {
            if (request.RoleNames?.Length == 0)
            {
                return BadRequest();
            }
            if (request.RoleNames.Length == 1 && request.RoleNames[0] == Constants.SystemConstants.Roles.Admin)
            {
                return base.BadRequest();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();
            var result = await _userManager.RemoveFromRolesAsync(user, request.RoleNames);
            if (result.Succeeded)
                return Ok();

            return BadRequest();
        }

        #endregion
    }
}