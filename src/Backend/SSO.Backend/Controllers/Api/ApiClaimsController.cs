using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Services.RequestModel.Api;
using SSO.Services.ViewModel.ApiResource;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Api
{
    public partial class ApiResourcesController
    {
        #region Apiclaims
        //Get api claims for api with id
        [HttpGet("{id}/apiClaims")]
        public async Task<IActionResult> GetApiClaims(int id)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            if (apiResource == null)
            {
                return NotFound();
            }
            var query = _context.ApiClaims.AsQueryable();
            query = query.Where(x => x.ApiResourceId == apiResource.Id);
            var apiClaimsViewModel = query.Select(x => new ApiClaimsViewModel()
            {
                Id = x.Id,
                Type = x.Type,
                ApiResourceId = x.ApiResourceId
            });

            return Ok(apiClaimsViewModel);
        }

        //Post new api claim for api with id
        [HttpPost("{id}/apiClaims")]
        public async Task<IActionResult> PostApiClaim(int id, [FromBody]ApiClaimRequest request)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            apiResource.Updated = DateTime.UtcNow;
            var apiClaim = await _context.ApiClaims.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id);
            var apiClaimRequest = new ApiResourceClaim()
            {
                Type = apiClaim.Type,
                ApiResourceId = request.ApiResourceId
            };
            _context.ApiClaims.Add(apiClaimRequest);
            var update = DateTime.UtcNow;
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                _context.ApiResources.Update(apiResource);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest();
        }

        //Delete api claim for api with api id
        [HttpDelete("{id}/apiClaims/{claimId}")]
        public async Task<IActionResult> DeleteApiClaim(int id, int claimId)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            if (apiResource == null)
            {
                return NotFound();
            }
            apiResource.Updated = DateTime.UtcNow;
            var apiClaim = await _context.ApiClaims.FirstOrDefaultAsync(x => x.Id == claimId && x.ApiResourceId == apiResource.Id);
            if (apiClaim == null)
            {
                return NotFound();
            }
            _context.ApiClaims.Remove(apiClaim);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                _context.ApiResources.Update(apiResource);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        #endregion
    }
}