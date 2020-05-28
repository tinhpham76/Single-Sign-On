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
        #region Api Scope Claims
        //Get api scope claim
        [HttpGet("{apiResourceName}/apiScopes/{scopeName}/scopeClaims")]
        public async Task<IActionResult> GetApiScopeClaims(string apiResourceName)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            var apiScope = _context.ApiScopes.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id);
            var query = _context.ApiScopeClaims.Where(x => x.ApiScopeId.Equals(apiScope.Id));
            var apiScopeClaims = await query.Select(x => new ApiScopeClaimsViewModel()
            {
                Type = x.Type
            }).ToListAsync();

            return Ok(apiScopeClaims);
        }

        //Post api scope claim
        [HttpPost("{apiResourceName}/apiScopes/{scopeName}/scopeClaims")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PostApiScopeClaim(string apiResourceName, string scopeName, [FromBody]ApiScopeClaimRequest request)
        {
            //Check Api Resource
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            if (apiResource != null)
            {
                var apiScope = await _context.ApiScopes.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id && x.Name == scopeName);
                if (apiScope != null)
                {
                    var apiScopeClaim = await _context.ApiScopeClaims.FirstOrDefaultAsync(x => x.ApiScopeId == apiScope.Id);
                    if (apiScopeClaim == null)
                    {
                        var apiScopeClaimRequest = new IdentityServer4.EntityFramework.Entities.ApiScopeClaim()
                        {
                            Type = request.Type,
                            ApiScopeId = apiScope.Id
                        };
                        _context.ApiScopeClaims.Add(apiScopeClaimRequest);
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
                    else if (apiScopeClaim != null)
                    {
                        if (apiScopeClaim.Type == request.Type)
                        {
                            return BadRequest();
                        }
                        else
                        {
                            var apiScopeClaimRequest = new IdentityServer4.EntityFramework.Entities.ApiScopeClaim()
                            {
                                Type = request.Type,
                                ApiScopeId = apiScope.Id
                            };
                            _context.ApiScopeClaims.Add(apiScopeClaimRequest);
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
                }
                return BadRequest();
            }
            return BadRequest();

        }
        #endregion
    }
}
