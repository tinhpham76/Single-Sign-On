using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Authorization;
using SSO.Backend.Constants;
using SSO.Services.RequestModel.Api;
using SSO.Services.ViewModel.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Api
{
    public partial class ApiScopesController
    {
        #region Api Scope Claims
        //Get api scope claim
        [HttpGet("{apiScopeName}/scopeClaims")]
        public async Task<IActionResult> GetApiScopeClaims(string apiScopeName)
        {
            var apiScope = await _configurationDbContext.ApiScopes.FirstOrDefaultAsync(x => x.Name == apiScopeName);
            var apiClaims = await _context.ApiScopeClaims.FirstOrDefaultAsync(x => x.ScopeId == apiScope.Id);
            var apiScopeClaims = await _context.ApiScopeClaims.Where(x => x.ScopeId == apiScope.Id)
                                                        .Select(x => new List<string>()
                                                        {
                                                            x.Type
                                                        }).ToListAsync();
            return Ok(apiScopeClaims);
        }

        //Post api scope claim
        [HttpPost("{apiScopeName}/scopeClaims")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PostApiScopeClaim(string apiScopeName, [FromBody] ApiScopeClaimRequest request)
        {
            //Check Api scope
            var apiScope = await _configurationDbContext.ApiScopes.FirstOrDefaultAsync(x => x.Name == apiScopeName);
            if (apiScope != null)
            {
                var apiScopeClaim = await _context.ApiScopeClaims.FirstOrDefaultAsync(x => x.ScopeId == apiScope.Id);
                if (apiScopeClaim == null)
                {
                    var apiScopeClaimRequest = new IdentityServer4.EntityFramework.Entities.ApiScopeClaim()
                    {
                        Type = request.Type,
                        ScopeId = apiScope.Id
                    };
                    _context.ApiScopeClaims.Add(apiScopeClaimRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
                else if(apiScopeClaim != null)
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
                            ScopeId = apiScope.Id
                        };
                        _context.ApiScopeClaims.Add(apiScopeClaimRequest);
                        var result = await _context.SaveChangesAsync();
                        if (result > 0)
                        {
                            return Ok();
                        }
                        return BadRequest();
                    }
                }
            }
            return BadRequest();
        }

        //Delete api scope claim
        [RoleRequirement(RoleCode.Admin)]
        [HttpDelete("{apiScopeName}/scopeClaims/{claimType}")]
        public async Task<IActionResult> DeleteApiClaim(string apiScopeName, string claimType)
        {
            var apiScope = await _configurationDbContext.ApiScopes.FirstOrDefaultAsync(x => x.Name == apiScopeName);
            if (apiScope == null)
                return NotFound();
            var apiClaim = await _context.ApiScopeClaims.FirstOrDefaultAsync(x => x.ScopeId == apiScope.Id && x.Type == claimType);
            if (apiClaim == null)
                return NotFound();
            _context.ApiScopeClaims.Remove(apiClaim);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
        #endregion
    }
}
