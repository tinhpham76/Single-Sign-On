using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Authorization;
using SSO.Backend.Constants;
using SSO.Backend.Data;
using SSO.Services;
using SSO.Services.RequestModel.Identity;
using SSO.Services.ViewModel.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Identity
{

    public partial class IdentityResourcesController : BaseController
    {
        #region Identity Resource
        private readonly ConfigurationDbContext _configurationDbContext;
        private readonly ApplicationDbContext _context;
        public IdentityResourcesController(ConfigurationDbContext configurationDbContext,
            ApplicationDbContext context)
        {
            _configurationDbContext = configurationDbContext;
            _context = context;
        }
        //Get Basic infor identity resources
        [HttpGet]
        public async Task<IActionResult> GetIdentityResources()
        {
            var identityResources = await _configurationDbContext.IdentityResources.Select(x => new IdentityResourcesQuickView()
            {
                Name = x.Name,
                Description = x.Description
            }).ToListAsync();
            return Ok(identityResources);
        }

        //Find identity resource
        [HttpGet("filter")]
        public async Task<IActionResult> GetIdentityResourcesPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _configurationDbContext.ApiResources.AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.Contains(filter));
            }
            var totalReconds = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new IdentityResourcesQuickView()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToListAsync();
            var pagination = new Pagination<IdentityResourcesQuickView>
            {
                Items = items,
                TotalRecords = totalReconds
            };
            return Ok(pagination);
        }

        //Get detail info identity resource
        [HttpGet("{identityResourceName}")]
        public async Task<IActionResult> GetIdentityResource(string identityResourceName)
        {
            var identityResource = await _configurationDbContext.IdentityResources.FirstOrDefaultAsync(x => x.Name == identityResourceName);
            if (identityResource == null)
                return NotFound();
            var identityResourceViewModel = new IdentityResourceViewModel()
            {
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Enabled = identityResource.Enabled,
                ShowInDiscoveryDocument = identityResource.ShowInDiscoveryDocument,
                Required = identityResource.Required,
                Emphasize = identityResource.Emphasize
            };
            return Ok(identityResourceViewModel);
        }

        //Post identity resource
        [HttpPost]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PostIdentityResource([FromBody]IdentityResourceRequest request)
        {
            var identityResource = await _configurationDbContext.IdentityResources.FirstOrDefaultAsync(x => x.Name == request.Name);
            if (identityResource != null)
                return BadRequest($"Identity Resource name {request.Name} already exist!");
            var identityResourceRequest = new IdentityResource()
            {
                Name = request.Name,
                DisplayName = request.DisplayName,
                Description = request.Description,
                Enabled = request.Enabled,
                ShowInDiscoveryDocument = request.ShowInDiscoveryDocument,
                Required = request.Required,
                Emphasize = request.Emphasize
            };
            _configurationDbContext.IdentityResources.Add(identityResourceRequest.ToEntity());
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }

        //Put Identity Resource 
        [HttpPut("{identityResourceName}")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PutIdentityResource(string identityResourceName, [FromBody]IdentityResourceRequest request)
        {
            var identityResource = await _configurationDbContext.IdentityResources.FirstOrDefaultAsync(x => x.Name == identityResourceName);
            if (identityResource == null)
                return NotFound();
            identityResource.Name = request.Name;
            identityResource.DisplayName = request.DisplayName;
            identityResource.Description = request.Description;
            identityResource.Enabled = request.Enabled;
            identityResource.ShowInDiscoveryDocument = request.ShowInDiscoveryDocument;
            identityResource.Required = request.Required;
            identityResource.Emphasize = request.Emphasize;
            identityResource.Updated = DateTime.UtcNow;

            _configurationDbContext.IdentityResources.Update(identityResource);
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }

        //Delete Identity Resource
        [HttpDelete("{identityResourceName}")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> DeleteIdentityResource(string identityResourceName)
        {
            var identityResource = await _configurationDbContext.IdentityResources.FirstOrDefaultAsync(x => x.Name == identityResourceName);
            if (identityResource == null)
                return NotFound();
            _configurationDbContext.IdentityResources.Remove(identityResource);
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }
        #endregion
    }
}