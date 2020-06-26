
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Authorization;
using SSO.Backend.Constants;
using SSO.Backend.Data;
using SSO.Services;
using SSO.Services.RequestModel.Api;
using SSO.Services.ViewModel.Api;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Api
{
    public partial class ApiScopesController : BaseController
    {
        #region Api Scopes
        private readonly ConfigurationDbContext _configurationDbContext;
        private readonly ApplicationDbContext _context;
        public ApiScopesController(ApplicationDbContext context,
            ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
            _context = context;
        }

        // Find api scopes with scope name or id
        [HttpGet("filter")]
        public async Task<IActionResult> GetApiScopesPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _configurationDbContext.ApiScopes.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.Contains(filter));
            }
            var totalReconds = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ApiScopesQuickView()
                {
                    Name = x.Name,
                    Description = x.Description,
                    DisplayName = x.DisplayName
                }).ToListAsync();

            var pagination = new Pagination<ApiScopesQuickView>
            {
                Items = items,
                TotalRecords = totalReconds
            };
            return Ok(pagination);
        }

        //Get infor api scope
        [HttpGet("{apiScopeName}")]
        public async Task<IActionResult> GetApiScope(string apiScopeName)
        {
            var apiScope= await _configurationDbContext.ApiScopes.FirstOrDefaultAsync(x => x.Name == apiScopeName);
            if (apiScope == null)
                return NotFound();
            var apiScopeViewModel = new ApiScopeViewModel()
            {                
                Name = apiScope.Name,
                DisplayName = apiScope.DisplayName,
                Description = apiScope.Description,
                Enabled = apiScope.Enabled,
                ShowInDiscoveryDocument = apiScope.ShowInDiscoveryDocument,
                Emphasize = apiScope.Emphasize,
                Required = apiScope.Required
            };
            return Ok(apiScopeViewModel);
        }

        //Post basic infor api scope
        [HttpPost]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PostApiScope([FromBody]ApiScopeRequest request)
        {
            var apiScope = await _configurationDbContext.ApiScopes.FirstOrDefaultAsync(x => x.Name == request.Name);
            if (apiScope != null)
                return BadRequest($"Api Resource name {request.Name} already exist");
            var apiApiScopeRequest = new ApiScope()
            {
                Name = request.Name,
                DisplayName = request.DisplayName,
                Description = request.Description,
                Enabled = request.Enabled,
                ShowInDiscoveryDocument = request.ShowInDiscoveryDocument,
                Emphasize = request.Emphasize,
                Required = request.Required                
            };
            _configurationDbContext.ApiScopes.Add(apiApiScopeRequest.ToEntity());
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }

        //Edit basic infor api scope
        [HttpPut("{apiScopeName}")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PutApiScope(string apiScopeName, [FromBody] ApiScopeRequest request)
        {
            var apiScope = await _configurationDbContext.ApiScopes.FirstOrDefaultAsync(x => x.Name == apiScopeName);
            if (apiScope == null)
                return BadRequest();
            apiScope.Enabled = request.Enabled;
            apiScope.Name = request.Name;
            apiScope.DisplayName = request.DisplayName;
            apiScope.Description = request.Description;
            apiScope.Required = request.Required;
            apiScope.Emphasize = request.Emphasize;
            apiScope.ShowInDiscoveryDocument = request.ShowInDiscoveryDocument;
            _configurationDbContext.ApiScopes.Update(apiScope);
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }

        //Delete api scope
        [HttpDelete("{apiScopeName}")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> DeleteApiScope(string apiScopeName)
        {
            var apiScope = await _configurationDbContext.ApiScopes.FirstOrDefaultAsync(x => x.Name == apiScopeName);
            if (apiScope == null)
                return NotFound();
            _configurationDbContext.ApiScopes.Remove(apiScope);
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }
        #endregion
    }
}
