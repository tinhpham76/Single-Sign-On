using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Authorization;
using SSO.Backend.Constants;
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
        [ClaimRequirement(PermissionCode.SSO_VIEW)]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.Select(x => x.Id.ToString()).ToListAsync();

            return Ok(roles);
        }

        [HttpGet("{roleId}")]
        [ClaimRequirement(PermissionCode.SSO_VIEW)]
        public async Task<IActionResult> GetRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound();
            var roleViewModel = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName
            };
            return Ok(roleViewModel);
        }

        [HttpGet("filter")]
        [ClaimRequirement(PermissionCode.SSO_VIEW)]
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
                    Id = x.Id,
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
        [ClaimRequirement(PermissionCode.SSO_CREATE)]
        public async Task<IActionResult> PostRole([FromBody] RoleRequest request)
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
        [ClaimRequirement(PermissionCode.SSO_UPDATE)]
        public async Task<IActionResult> PutRole(string roleId, [FromBody] RoleRequest request)
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
        [ClaimRequirement(PermissionCode.SSO_DELETE)]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound();

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
                return Ok();
            return BadRequest();
        }
    }
}