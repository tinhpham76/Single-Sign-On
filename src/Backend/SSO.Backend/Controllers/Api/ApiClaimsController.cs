using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Services.RequestModel.Api;
using SSO.Services.ViewModel.Api;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Api
{
    public partial class ApiResourcesController
    {
        #region Api Claim
        //Get api claim
        [HttpGet("{apiResourceName}/apiClaims")]
        public async Task<IActionResult> GetApiClaim(string apiResourceName)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            var query = _context.ApiClaims.Where(x => x.ApiResourceId.Equals(apiResource.Id));
            var apiClaims = await query.Select(x => new ApiClaimsViewModel()
            {
                Type = x.Type
            }).ToListAsync();

            return Ok(apiClaims);
        }

        //Post api claim
        [HttpPost("{apiResourceName}/apiClaims")]
        public async Task<IActionResult> PostApiClaims(string apiResourceName, [FromBody]ApiClaimRequest request)
        {
            //Check Api Resource
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            //If api resource not null, Check api claims
            if (apiResource != null)
            {
                var apiClaim = await _context.ApiClaims.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id);
                //If api claim is null, add claim for client
                if (apiClaim == null)
                {
                    var apiClaimRequest = new IdentityServer4.EntityFramework.Entities.ApiResourceClaim()
                    {
                        Type = request.Type,
                        ApiResourceId = apiResource.Id
                    };
                    _context.ApiClaims.Add(apiClaimRequest);
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
                    _context.ApiClaims.Add(apiClaimRequest);
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

        //Delete api claim
        [HttpDelete("{apiResourceName}/apiClaims/{claimType}")]
        public async Task<IActionResult> DeleteApiClaim(string apiResourceName, string claimType)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            if (apiResource == null)
                return NotFound();
            var apiClaim = await _context.ApiClaims.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id && x.Type == claimType);
            if (apiClaim == null)
                return NotFound();
            _context.ApiClaims.Remove(apiClaim);
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
