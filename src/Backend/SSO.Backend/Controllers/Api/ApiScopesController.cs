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
        #region ApiProperties
        //Get api scope for api with id
        [HttpGet("{id}/apiScopes")]
        public async Task<IActionResult> GetApiScopes(int id)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            if (apiResource == null)
            {
                return NotFound();
            }
            var query = _context.ApiScopes.AsQueryable();
            query = query.Where(x => x.ApiResourceId == apiResource.Id);
            var apiScopesViewModel = query.Select(x => new ApiScopesViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                DisplayName = x.DisplayName,
                Description = x.Description,
                Emphasize = x.Emphasize,
                Required = x.Required,
                ShowInDiscoveryDocument = x.ShowInDiscoveryDocument,
                ApiResourceId = x.ApiResourceId
            });

            return Ok(apiScopesViewModel);
        }

        //Post new api scope for api resource with id
        [HttpPost("{id}/apiScopes")]
        public async Task<IActionResult> PostApiScope(int id, [FromBody]ApiScopeRequest request)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            apiResource.Updated = DateTime.UtcNow;
            var apiSope = await _context.ApiScopes.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id);
            var apiScopeRequest = new ApiScope()
            {
                Name = request.Name,
                DisplayName = request.DisplayName,
                Description = request.Description,
                Emphasize = request.Emphasize,
                Required = request.Required,
                ShowInDiscoveryDocument = request.ShowInDiscoveryDocument,
                ApiResourceId = request.ApiResourceId
            };
            _context.ApiScopes.Add(apiScopeRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                _context.ApiResources.Update(apiResource);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest();
        }

        //Delete api scope for api resource with api id
        [HttpDelete("{id}/apiScopes/{scopeId}")]
        public async Task<IActionResult> DeleteApiScope(int id, int scopeId)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            if (apiResource == null)
            {
                return NotFound();
            }
            apiResource.Updated = DateTime.UtcNow;
            var apiScope = await _context.ApiScopes.FirstOrDefaultAsync(x => x.Id == scopeId && x.ApiResourceId == apiResource.Id);
            if (apiScope == null)
            {
                return NotFound();
            }
            _context.ApiScopes.Remove(apiScope);
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