using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Services.RequestModel.Identity;
using SSO.Services.ViewModel.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Identity
{
    public partial class IdentityResourcesController
    {
        #region Identity Resource Claim
        //Get ientity resource claim
        [HttpGet("{identityResourceName}/identityClaims")]
        public async Task<IActionResult> GetIdentityClaims(string identityResourceName)
        {
            var identityResource = await _configurationDbContext.IdentityResources.FirstOrDefaultAsync(x => x.Name == identityResourceName);
            if (identityResource == null)
                return NotFound();
            var query = _context.IdentityClaims.Where(x => x.IdentityResourceId.Equals(identityResource.Id));
            var identityClaims = await query.Select(x => new IdentityClaimsViewModel()
            {
                Type = x.Type
            }).ToListAsync();

            return Ok(identityClaims);
        }

        //Post identity claim
        [HttpPost("{identityResourceName}/identityClaims")]
        public async Task<IActionResult> PostIdentityClaim(string identityResourceName, [FromBody]IdentityClaimRequest request)
        {
            var identityResource = await _configurationDbContext.IdentityResources.FirstOrDefaultAsync(x => x.Name == identityResourceName);
            if (identityResource != null)
            {
                var identityClaim = await _context.IdentityClaims.FirstOrDefaultAsync(x => x.IdentityResourceId == identityResource.Id);
                if (identityClaim == null)
                {
                    var identityClaimRequest = new IdentityClaim()
                    {
                        Type = request.Type,
                        IdentityResourceId = identityResource.Id
                    };
                    _context.IdentityClaims.Add(identityClaimRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                        return Ok();
                    return BadRequest();
                }
                else if (identityClaim != null)
                {
                    if (identityClaim.Type == request.Type)
                    {
                        return BadRequest($"Identity Claim type {request.Type} already exist!");
                    }
                    else
                    {
                        var identityClaimRequest = new IdentityClaim()
                        {
                            Type = request.Type,
                            IdentityResourceId = identityResource.Id
                        };
                        _context.IdentityClaims.Add(identityClaimRequest);
                        var result = await _context.SaveChangesAsync();
                        if (result > 0)
                            return Ok();
                        return BadRequest();
                    }
                }

            }
            return BadRequest();
        }

        //Delete identity claim
        [HttpDelete("{identityResourceName}/identityClaims/{claimType}")]
        public async Task<IActionResult> DeleteApiClaim(string identityResourceName, string claimType)
        {
            var identityResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == identityResourceName);
            if (identityResource == null)
                return NotFound();
            var identityClaim = await _context.IdentityClaims.FirstOrDefaultAsync(x => x.IdentityResourceId == identityResource.Id && x.Type == claimType);
            if (identityClaim == null)
                return NotFound();
            _context.IdentityClaims.Remove(identityClaim);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();

        }
        #endregion
    }
}
