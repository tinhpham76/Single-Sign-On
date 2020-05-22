using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Services.RequestModel.ApiResource;
using SSO.Services.ViewModel.ApiResource;

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

        //Post new api claim for api resource with id
        [HttpPost("{id}/apiClaims")]
        public async Task<IActionResult> PostApiClaim(int id, [FromBody]ApiClaimRequest request)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            var apiClaim = await _context.ApiClaims.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id);
            var apiClaimRequest = new ApiResourceClaim()
            {
                Type = apiClaim.Type,                
                ApiResourceId = request.ApiResourceId
            };
            _context.ApiClaims.Add(apiClaimRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }

        //Delete api claim for api claim with api id
        [HttpDelete("{id}/apiClaims/{claimId}")]
        public async Task<IActionResult> DeleteApiClaim(int id, int claimId)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            if (apiResource == null)
            {
                return NotFound();
            }
            var apiClaim = await _context.ApiClaims.FirstOrDefaultAsync(x => x.Id == claimId && x.ApiResourceId == apiResource.Id);
            if (apiClaim == null)
            {
                return NotFound();
            }
            _context.ApiClaims.Remove(apiClaim);
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