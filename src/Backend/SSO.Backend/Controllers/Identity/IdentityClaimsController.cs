using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Services.RequestModel.Identity;
using SSO.Services.ViewModel.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Identity
{
    public partial class IdentityResourcesController
    {
        #region IdentityClaim
        //Get claim for identity with id
        [HttpGet("{id}/identityClaims")]
        public async Task<IActionResult> GetIdentityClaims(int id)
        {
            var identityResource = await _context.IdentityResources.FirstOrDefaultAsync(x => x.Id == id);
            if (identityResource == null)
            {
                return NotFound();
            }
            var query = _context.IdentityClaims.AsQueryable();
            query = query.Where(x => x.IdentityResourceId == identityResource.Id);
            var identityClaimsViewModel = query.Select(x => new IdentityClaimsViewModel()
            {
                Id = x.Id,
                Type = x.Type,
                IdentityResourceId = identityResource.Id
            });

            return Ok(identityClaimsViewModel);
        }

        //Post claim for identity resource with id
        [HttpPost("{id}/identityClaims")]
        public async Task<IActionResult> PostIdentityClaim(int id, [FromBody]IdentityClaimRequest request)
        {
            var identityResource = await _context.IdentityResources.FirstOrDefaultAsync(x => x.Id == id);
            identityResource.Updated = DateTime.UtcNow;
            var identityClaimRequest = new IdentityClaim()
            {
                Type = request.Type,
                IdentityResourceId = request.IdentityResourceId
            };
            _context.IdentityClaims.Add(identityClaimRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                _context.IdentityResources.Update(identityResource);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest();
        }

        //Delete Origins for client with client id
        [HttpDelete("{id}/identityClaims/{claimId}")]
        public async Task<IActionResult> DeleteIdentityClaim(int id, int claimId)
        {
            var identityResource = await _context.IdentityResources.FirstOrDefaultAsync(x => x.Id == id);
            if (identityResource == null)
            {
                return NotFound();
            }
            identityResource.Updated = DateTime.UtcNow;
            var identityClaim = await _context.IdentityClaims.FirstOrDefaultAsync(x => x.Id == claimId && x.IdentityResourceId == identityResource.Id);
            if (identityClaim == null)
            {
                return NotFound();
            }
            _context.IdentityClaims.Remove(identityClaim);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {

                _context.IdentityResources.Update(identityResource);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        #endregion
    }
}