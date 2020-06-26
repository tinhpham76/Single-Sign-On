using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Authorization;
using SSO.Backend.Constants;
using SSO.Services.RequestModel.Api;
using SSO.Services.ViewModel.Api;

namespace SSO.Backend.Controllers.Api
{
    public partial class ApiResourcesController
    {
        #region Api Resource Scopes
        //Get api resource scopes
        [HttpGet("{apiResourceName}/apiResourceScopes")]
        public async Task<IActionResult> GetApiResourceScopes(string apiResourceName)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            if (apiResource == null)
                return NotFound();

            var allScopes =await _configurationDbContext.ApiResources.Select(x => x.Name.ToString()).ToListAsync();
          
            var query = _context.ApiResourceScopes.Where(x => x.ApiResourceId.Equals(apiResource.Id));
            var apiScopes = await query.Select(x => new ApiResourceScopeViewModel()
            {
                Label = x.Scope,
                Value = x.Scope,
                Checked = allScopes.Contains(x.Scope) ? true : false,
                Disabled = false,
                Name = x.Scope
            }).ToListAsync();

            return Ok(apiScopes);
        }

        //Post api resource scope
        [HttpPost("{apiResourceName}/apiResourceScopes")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PostApiResourceScope(string apiResourceName, [FromBody] ApiResourceScopeRequest request)
        {
            //Check Api Resource
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            //If api resource not null, Check api claims
            if (apiResource != null)
            {
                var apiScope = await _context.ApiResourceScopes.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id);
                //If api claim is null, add claim for client
                if (apiScope == null)
                {
                    var apiScopeRequest = new IdentityServer4.EntityFramework.Entities.ApiResourceScope()
                    {
                        Scope = request.Scope,
                        ApiResourceId = apiResource.Id
                    };
                    _context.ApiResourceScopes.Add(apiScopeRequest);
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
                // If api claim not null, Check api claim type on table with request claim type
                else if (apiScope != null)
                {
                    if (apiScope.Scope == request.Scope)
                        return BadRequest($"Api resource scope {request.Scope} already exist");

                    var apiScopeRequest = new IdentityServer4.EntityFramework.Entities.ApiResourceScope()
                    {
                        Scope = request.Scope,
                        ApiResourceId = apiResource.Id
                    };
                    _context.ApiResourceScopes.Add(apiScopeRequest);
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

        //Delete api resource scope
        [RoleRequirement(RoleCode.Admin)]
        [HttpDelete("{apiResourceName}/apiResourceScopes/{scopeName}")]
        public async Task<IActionResult> DeleteApiResourceScope(string apiResourceName, string scopeName)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            if (apiResource == null)
                return NotFound();
            var apiScope = await _context.ApiResourceScopes.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id && x.Scope == scopeName);
            if (apiScope == null)
                return NotFound();
            _context.ApiResourceScopes.Remove(apiScope);
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
