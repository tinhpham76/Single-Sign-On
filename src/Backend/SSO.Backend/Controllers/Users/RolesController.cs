﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Data;
using SSO.Services;
using SSO.Services.RequestModel.User;
using SSO.Services.ViewModel.User;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Users
{
    public class RolesController : BaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public RolesController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        //get all role
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = _roleManager.Roles;
            var rolesQuickView = await roles.Select(x => new RolesQuickView()
            {
                Name = x.Name,
                NormalizedName = x.NormalizedName
            }).ToListAsync();
            return Ok(rolesQuickView);
        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound();
            var roleViewModel = new RoleViewModel()
            {
                Name = role.Name,
                NormalizedName = role.NormalizedName
            };
            return Ok(roleViewModel);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetRolePaging(string filter, int pageIndex, int pageSize)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.Contains(filter));
            }
            var totalReconds = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new RolesQuickView()
                {
                    Name = x.Name,
                    NormalizedName = x.NormalizedName
                }).ToListAsync();
            var pagination = new Pagination<RolesQuickView>
            {
                TotalRecords = totalReconds,
                Items = items
            };
            return Ok(pagination);
        }

        [HttpPost]
        public async Task<IActionResult> PostRole([FromBody]RoleRequest request)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Name == request.Name);
            if (role != null)
                return BadRequest($"Role name {request.Name} already exist!");
            var roleRequest = new IdentityRole()
            {
                Id = request.Id,
                Name = request.Name,
                NormalizedName = request.Name.ToUpper()
            };
            var result = await _roleManager.CreateAsync(roleRequest);
            if (result.Succeeded)
                return Ok();
            return BadRequest();
        }

        [HttpPut("{roleId}")]
        public async Task<IActionResult> PutRole(string roleId, [FromBody]RoleRequest request)
        {
            if (roleId != request.Id)
                return BadRequest("Role id not match");

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound();

            role.Name = request.Name;
            role.NormalizedName = request.Name.ToUpper();

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
                return Ok();
            return BadRequest();
        }
    }
}