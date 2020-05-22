using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Data;
using SSO.Service.RequestModel.IdentityResource;
using SSO.Services.RequestModel.IdentityResource;
using SSO.Services.ViewModel.IdentityResource;

namespace SSO.Backend.Controllers.Identity
{
    public partial class IdentityResourcesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ConfigurationDbContext _configurationDbContext;
        public IdentityResourcesController(ConfigurationDbContext configurationDbContext,
            ApplicationDbContext context)
        {
            _context = context;
            _configurationDbContext = configurationDbContext;
        }

        #region IdentityResources
        //Get Identity Resources basic info
        [HttpGet]
        public async Task<IActionResult> GetIdentityResources()
        {
            var identityResources = _configurationDbContext.IdentityResources;
            var identityResourcesQuickView = await identityResources.Select(x => new IdentityResourceQuickView() {
                Enable = x.Enabled,
                Name = x.Name,
                DisplayName = x.DisplayName
            }).ToListAsync();
            return Ok(identityResourcesQuickView);
        }

        //Get detail info identity resource with identiy resource id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailIdentityResource(int id)
        {
            var identityResource = await _configurationDbContext.IdentityResources.FirstOrDefaultAsync(x => x.Id == id);
            if(identityResource == null)
            {
                return NotFound();
            }
            var identityResourceViewModel = new IdentityResourceViewModel()
            {
                Id = identityResource.Id,
                Enabled = identityResource.Enabled,
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Created = identityResource.Created,
                Updated = identityResource.Updated,
                Emphasize = identityResource.Emphasize,
                NonEditable = identityResource.NonEditable,
                Required = identityResource.Required,
                ShowInDiscoveryDocument = identityResource.ShowInDiscoveryDocument
            };
            return Ok(identityResourceViewModel);
        }

        //Post new Identity Resource
        [HttpPost]
        public async Task<IActionResult> PostIdentityResource([FromBody]IdentityResourceQuickRequest request)
        {
            var identityResource = await _context.IdentityResources.FirstOrDefaultAsync(x => x.Name == request.Name);
            if(identityResource != null)
            {
                return BadRequest($"Identity Resource with name {request.Name} already exist");
            }
            var identityResourceRequest = new IdentityResource()
            {
               
                Name = request.Name,
                DisplayName = request.DisplayName,
                Description = request.Description,
            };
            _configurationDbContext.IdentityResources.Add(identityResourceRequest.ToEntity());
            var result = await _configurationDbContext.SaveChangesAsync();
            if(result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
        //Put Identity Resource
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIdentityResource(int id, [FromBody]IdentityResourceRequest request)
        {
            var identityResource = await _context.IdentityResources.FirstOrDefaultAsync(x => x.Id == id);
            if(identityResource == null)
            {
                return NotFound();
            }
            identityResource.Enabled = request.Enabled;
            identityResource.Name = request.Name;
            identityResource.DisplayName = request.DisplayName;
            identityResource.Description = request.Description;
            identityResource.Required = request.Required;
            identityResource.Emphasize = request.Emphasize;
            identityResource.ShowInDiscoveryDocument = request.ShowInDiscoveryDocument;
            identityResource.Updated = DateTime.UtcNow;

            _context.IdentityResources.Update(identityResource);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }

        //Delete Identity Resource
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIdentityResource(int id)
        {
            var identityResource = await _configurationDbContext.IdentityResources.FirstOrDefaultAsync(x => x.Id == id);
            if (identityResource == null)
            {
                return NotFound();
            }
            _configurationDbContext.IdentityResources.Remove(identityResource);
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
        

        #endregion
    }
}