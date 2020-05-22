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
        #region IdentityResourceProperties
        //Get identity property for identity resource with id
        [HttpGet("{id}/identityResourceProperties")]
        public async Task<IActionResult> GetIdentityResourceProperties(int id)
        {
            var identityResource = await _context.IdentityResources.FirstOrDefaultAsync(x => x.Id == id);
            if (identityResource == null)
            {
                return NotFound();
            }
            var query = _context.IdentityProperties.AsQueryable();
            query = query.Where(x => x.IdentityResourceId == identityResource.Id);
            var identityResourcePropertiesViewModel = query.Select(x => new IdentityResourcePropertiesViewModel()
            {
                Id = x.Id,
                Key = x.Key,
                Value = x.Value,
                IdentityResourceId = identityResource.Id
            });

            return Ok(identityResourcePropertiesViewModel);
        }

        //Post new identity property for identity resource with id
        [HttpPost("{id}/identityResourceProperties")]
        public async Task<IActionResult> PostIdentityResourceProperty(int id, [FromBody]IdentityResourcePropertyRequest request)
        {
            var identityResource = await _context.IdentityResources.FirstOrDefaultAsync(x => x.Id == id);
            identityResource.Updated = DateTime.UtcNow;
            var identityResourcePropertyRequest = new IdentityResourceProperty()
            {
                Key = request.Key,
                Value = request.Value,
                IdentityResourceId = request.IdentityResourceId
            };
            _context.IdentityProperties.Add(identityResourcePropertyRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                _context.IdentityResources.Update(identityResource);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest();
        }

        //Delete identity property for identity resource with id
        [HttpDelete("{id}/identityResourceProperties/{propertyId}")]
        public async Task<IActionResult> DeleletIdentityResourceProperty(int id, int propertyId)
        {
            var identityResource = await _context.IdentityResources.FirstOrDefaultAsync(x => x.Id == id);
            if (identityResource == null)
            {
                return NotFound();
            }
            identityResource.Updated = DateTime.UtcNow;
            var identityResourceProperty = await _context.IdentityProperties.FirstOrDefaultAsync(x => x.Id == propertyId && x.IdentityResourceId == identityResource.Id);
            if (identityResourceProperty == null)
            {
                _context.IdentityResources.Update(identityResource);
                await _context.SaveChangesAsync();
                return NotFound();
            }
            _context.IdentityProperties.Remove(identityResourceProperty);
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