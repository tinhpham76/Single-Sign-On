using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManager.Data;
using UserManager.Data.Entities;
using UserManager.Models;

namespace UserManager.Controllers
{
    public class UsersController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UsersController(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        //URL: GET: http://localhost:5001/api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, [FromBody] UserCreateRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Dob = DateTime.Parse(request.Dob);
            user.LastModifiedDate = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPut("{id}/change-password")]
        public async Task<IActionResult> PutUserPassword(string id, [FromBody] UserPasswordChangeRequest request)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
            var otherUsers = adminUsers.Where(x => x.Id != id).ToList();
            if (otherUsers.Count == 0)
            {
                return BadRequest();
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


    }
}
