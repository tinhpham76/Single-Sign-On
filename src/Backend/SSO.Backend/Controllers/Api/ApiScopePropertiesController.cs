using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Authorization;
using SSO.Backend.Constants;
using SSO.Services.RequestModel.Api;
using SSO.Services.ViewModel.Api;

namespace SSO.Backend.Controllers.Api
{
    public partial class ApiScopesController 
    {
        #region Api Scope Properties
        //Get api scope properties
        [HttpGet("{apiScopeName}/apiScopeProperties")]
        public async Task<IActionResult> GetApiScopeProperties(string apiScopeName)
        {

            var apiScope = await _configurationDbContext.ApiScopes.FirstOrDefaultAsync(x => x.Name == apiScopeName);
            if (apiScope == null)
                return NotFound();
            var query = _context.ApiScopeProperties.Where(x => x.ScopeId.Equals(apiScope.Id));
            var apiScopeProperties = await query.Select(x => new ApiScopePropertitesViewModel()
            {
                Id = x.Id,
                Key = x.Key,
                Value = x.Value
            }).ToListAsync();

            return Ok(apiScopeProperties);
        }

        //Post api resource property
        [HttpPost("{apiScopeName}/apiScopeProperties")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PostApiScopeProperty(string apiScopeName, [FromBody] ApiScopePropertyRequest request)
        {
            //Check Api scope
            var apiScope = await _configurationDbContext.ApiScopes.FirstOrDefaultAsync(x => x.Name == apiScopeName);
            //If api resource not null, Check api claims
            if (apiScope != null)
            {
                var apiScopeProperty = await _context.ApiResourceProperties.FirstOrDefaultAsync(x => x.ApiResourceId == apiScope.Id);
                //If api claim is null, add claim for client
                if (apiScopeProperty == null)
                {
                    var apiScopePropertyRequest = new IdentityServer4.EntityFramework.Entities.ApiScopeProperty()
                    {
                        Key = request.Key,
                        Value = request.Value,
                        ScopeId = apiScope.Id
                    };
                    _context.ApiScopeProperties.Add(apiScopePropertyRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        await _configurationDbContext.SaveChangesAsync();
                        return Ok();
                    }
                    return BadRequest();
                }
                // If api claim not null, Check api claim type on table with request claim type
                else if (apiScopeProperty != null)
                {
                    if (apiScopeProperty.Key == request.Key)
                        return BadRequest($"Api resource property key {request.Key} already exist");

                    var apiScopePropertyRequest = new IdentityServer4.EntityFramework.Entities.ApiScopeProperty()
                    {
                        Key = request.Key,
                        Value = request.Value,
                        ScopeId = apiScope.Id
                    };
                    _context.ApiScopeProperties.Add(apiScopePropertyRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        await _configurationDbContext.SaveChangesAsync();
                        return Ok();
                    }
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        //Delete api resource property
        [RoleRequirement(RoleCode.Admin)]
        [HttpDelete("{apiResourceName}/apiResourceProperties/{propertyKey}")]
        public async Task<IActionResult> DeleteApiResourceProperty(string apiResourceName, string propertyKey)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            if (apiResource == null)
                return NotFound();
            var apiProperty = await _context.ApiResourceProperties.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id && x.Key == propertyKey);
            if (apiProperty == null)
                return NotFound();
            _context.ApiResourceProperties.Remove(apiProperty);
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
