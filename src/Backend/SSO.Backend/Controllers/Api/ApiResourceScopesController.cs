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
        [HttpGet("{apiResourceName}/resourceScopes")]
        public async Task<IActionResult> GetApiResourceScopes(string apiResourceName)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            if (apiResource == null)
                return NotFound();

            var apiScopes = await _context.ApiResourceScopes
                .Where(x => x.ApiResourceId == apiResource.Id)
                .Select(x => x.Scope.ToString()).ToListAsync();
            if (apiScopes.Count == 0)
            {
                var allScope = await _configurationDbContext.ApiScopes.Select(x => new ApiResourceScopeViewModel()
                {
                    Label = x.Name,
                    Value = x.Name,
                    Checked = false,
                    Name = x.Name
                }).ToListAsync();
                return Ok(allScope);
            }
            var allScopes = await _configurationDbContext.ApiScopes.Select(x => new ApiResourceScopeViewModel()
            {
                Label = x.Name,
                Value = x.Name,
                Checked = apiScopes.Contains(x.Name) ? true : false,
                Name = x.Name
            }).ToListAsync();

            return Ok(allScopes);
        }

        //Post api resource scope
        [HttpPost("{apiResourceName}/resourceScopes")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PostApiResourceScope(string apiResourceName, [FromBody] ApiResourceScopeRequest request)
        {
            //Check Api Resource
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            //If api resource not null, Check api claims
            if (apiResource == null)
                return NotFound();
            var apiResourceScopes = await _context.ApiResourceScopes.Select(x => x.Scope.ToString()).ToListAsync();
            if (apiResourceScopes == null)
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
            else
            {
                if (!apiResourceScopes.Contains(request.Scope))
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
                else
                {
                    return BadRequest($"Scope {request.Scope} is already in another scope");
                }
            }
        }


        //Delete api resource scope
        [RoleRequirement(RoleCode.Admin)]
        [HttpDelete("{apiResourceName}/resourceScopes/{scopeName}")]
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
