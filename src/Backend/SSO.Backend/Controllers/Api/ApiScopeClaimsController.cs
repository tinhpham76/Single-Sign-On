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
        #region ApiScopeCliams
        //Get api scope claim for api scope with id
        [HttpGet("{id}/apiScopes/{scopeId}/apiScopeClaims")]
        public async Task<IActionResult> GetApiScopeClaims(int id, int scopeId)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            var apiScope = await _context.ApiScopes.FirstOrDefaultAsync(x => x.Id == scopeId);
            if ((apiResource == null && apiScope ==null) && (apiResource != null && apiScope == null))
            {
                return NotFound();                
            }            
            var query = _context.ApiScopeClaims.AsQueryable();
            query = query.Where(x => x.ApiScopeId == apiScope.Id);
            var apiScopeClaimsViewModel = query.Select(x => new ApiScopeClaimsViewModel()
            {
                Id = x.Id,
                Type = x.Type,
                ApiScopeId = x.ApiScopeId
            });

            return Ok(apiScopeClaimsViewModel);
        }

        //Post new api scope claim for api scope with id
        [HttpPost("{id}/apiScopes/{scopeId}/apiScopeClaims")]
        public async Task<IActionResult> PostApiProperty(int id, int scopeId, [FromBody]ApiScopeClaimRequest request)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            var apiScope = await _context.ApiScopes.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id);            
            var apiScopeClaimRequest = new ApiScopeClaim()
            {
                Type = request.Type,
                ApiScopeId = apiScope.Id
            };
            _context.ApiScopeClaims.Add(apiScopeClaimRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }

        //Delete api scope claim for api scope with api scope id
        [HttpDelete("{id}/apiScopes/{scopeId}/apiScopeClaims/{scopeClaimsId}")]
        public async Task<IActionResult> DeleteApiProperty(int id, int scopeId, int scopeClaimsId)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            var apiScope = await _context.ApiScopes.FirstOrDefaultAsync(x => x.Id == scopeId);
            if ((apiResource == null && apiScope == null) && (apiResource != null && apiScope == null))
            {
                return NotFound();
            }
            var apiScopeClaim = await _context.ApiScopeClaims.FirstOrDefaultAsync(x => x.Id == scopeClaimsId && x.ApiScopeId ==scopeId);
            if (apiScopeClaim == null)
            {
                return NotFound();
            }
            _context.ApiScopeClaims.Remove(apiScopeClaim);
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