using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Authorization;
using SSO.Backend.Constants;
using SSO.Services.RequestModel.Api;
using SSO.Services.ViewModel.Api;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Api
{
    public partial class ApiResourcesController
    {
        #region Api Scope
        //Get Api Scope
        [HttpGet("{apiResourceName}/apiScopes")]
        public async Task<IActionResult> GetApiScopes(string apiResourceName)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            var query = _context.ApiScopes.Where(x => x.ApiResourceId.Equals(apiResource.Id));
            var apiScopes = await query.Select(x => new ApiScopesViewModel()
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                Description = x.Description,
                Required = x.Required,
                ShowInDiscoveryDocument = x.ShowInDiscoveryDocument,
                Emphasize = x.Emphasize
            }).ToListAsync();

            return Ok(apiScopes);
        }

        //Post api scope
        [HttpPost("{apiResourceName}/apiScopes")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PostApiScope(string apiResourceName, [FromBody]ApiScopeRequest request)
        {
            //Check api resource
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            //If api resource not null, Check api resource scope
            if (apiResource != null)
            {
                var apiScope = await _context.ApiScopes.FirstOrDefaultAsync(x => x.Id == apiResource.Id);
                //If api resource scope is null, add scope for api
                if (apiScope == null)
                {
                    var apiScopeRequest = new IdentityServer4.EntityFramework.Entities.ApiScope()
                    {
                        Name = request.Name,
                        DisplayName = request.DisplayName,
                        Description = request.Description,
                        Required = request.Required,
                        Emphasize = request.Emphasize,
                        ShowInDiscoveryDocument = request.ShowInDiscoveryDocument,
                        ApiResourceId = apiResource.Id
                    };
                    _context.ApiScopes.Add(apiScopeRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        apiResource.Updated = DateTime.UtcNow;
                        _configurationDbContext.ApiResources.Update(apiResource);
                        await _configurationDbContext.SaveChangesAsync();
                        return Ok();
                    }
                    return BadRequest();
                }
                // If api resource scope not null, Check api resource scope name on table with request scope name
                else if (apiScope != null)
                {
                    if (apiScope.Name == request.Name)
                        return BadRequest($"Api Resource Scope Name {request.Name} already exist");

                    var apiScopeRequest = new IdentityServer4.EntityFramework.Entities.ApiScope()
                    {
                        Name = request.Name,
                        DisplayName = request.DisplayName,
                        Description = request.Description,
                        Required = request.Required,
                        Emphasize = request.Emphasize,
                        ShowInDiscoveryDocument = request.ShowInDiscoveryDocument,
                        ApiResourceId = apiResource.Id
                    };
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        apiResource.Updated = DateTime.UtcNow;
                        _configurationDbContext.ApiResources.Update(apiResource);
                        await _configurationDbContext.SaveChangesAsync();
                        return Ok();
                    }
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        //Delete api scope
        [HttpDelete("{apiResourceName}/apiScopes/{scopeName}")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> DeleteApiScope(string apiResourceName, string scopeName)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            if (apiResource == null)
                return NotFound();
            var apiScope = await _context.ApiScopes.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id && x.Name == scopeName);
            if (apiScope == null)
                return NotFound();
            _context.ApiScopes.Remove(apiScope);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                apiResource.Updated = DateTime.UtcNow;
                _configurationDbContext.ApiResources.Update(apiResource);
                await _configurationDbContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();

        }
        #endregion
    }
}
