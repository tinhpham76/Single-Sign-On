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
    public partial class ApiResourcesController
    {
        #region Api Resource Claims
        //Get api resource claims
        [HttpGet("{apiResourceName}/apiResourceClaims")]
        public async Task<IActionResult> GetApiResourceClaims(string apiResourceName)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            if (apiResource == null)
                return NotFound();
            var query = _context.ApiResourceClaims.Where(x => x.ApiResourceId.Equals(apiResource.Id));
            var apiClaims = await query.Select(x => new List<string>()
            {
                x.Type
            }).ToListAsync();

            return Ok(apiClaims);
        }

        //Post api resource claim
        [HttpPost("{apiResourceName}/apiResourceClaims")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PostApiClaim(string apiResourceName, [FromBody]ApiResourceClaimRequest request)
        {
            //Check Api Resource
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            //If api resource not null, Check api claims
            if (apiResource != null)
            {
                var apiClaim = await _context.ApiResourceClaims.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id);
                //If api claim is null, add claim for client
                if (apiClaim == null)
                {
                    var apiClaimRequest = new IdentityServer4.EntityFramework.Entities.ApiResourceClaim()
                    {
                        Type = request.Type,
                        ApiResourceId = apiResource.Id
                    };
                    _context.ApiResourceClaims.Add(apiClaimRequest);
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
                else if (apiClaim != null)
                {
                    if (apiClaim.Type == request.Type)
                        return BadRequest($"Api Claim Type {request.Type} already exist");

                    var apiClaimRequest = new IdentityServer4.EntityFramework.Entities.ApiResourceClaim()
                    {
                        Type = request.Type,
                        ApiResourceId = apiResource.Id
                    };
                    _context.ApiResourceClaims.Add(apiClaimRequest);
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

        //Delete api resource claim
        [RoleRequirement(RoleCode.Admin)]
        [HttpDelete("{apiResourceName}/apiResourceClaims/{claimType}")]
        public async Task<IActionResult> DeleteApiClaim(string apiResourceName, string claimType)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            if (apiResource == null)
                return NotFound();
            var apiClaim = await _context.ApiResourceClaims.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id && x.Type == claimType);
            if (apiClaim == null)
                return NotFound();
            _context.ApiResourceClaims.Remove(apiClaim);
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
