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
        #region ApiProperties
        //Get api properties for api with id
        [HttpGet("{id}/apiProperties")]
        public async Task<IActionResult> GetApiProperties(int id)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            if (apiResource == null)
            {
                return NotFound();
            }
            var query = _context.ApiProperties.AsQueryable();
            query = query.Where(x => x.ApiResourceId == apiResource.Id);
            var apiPropertiesViewModel = query.Select(x => new ApiPropertiesViewModel()
            {
                Id = x.Id,
                Key = x.Key,
                Value =x.Value,
                ApiResourceId = x.ApiResourceId
            });

            return Ok(apiPropertiesViewModel);
        }

        //Post new api property for api resource with id
        [HttpPost("{id}/apiProperties")]
        public async Task<IActionResult> PostApiProperty(int id, [FromBody]ApiPropertyRequest request)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            var apiProperty = await _context.ApiProperties.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id);
            var apiResourceRequest = new ApiResourceProperty()
            {
               Key = request.Key,
               Value = request.Value,
               ApiResourceId =request.ApiResourceId
            };
            _context.ApiProperties.Add(apiResourceRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }

        //Delete api property for api resource with api id
        [HttpDelete("{id}/apiProperties/{propertyId}")]
        public async Task<IActionResult> DeleteApiProperty(int id, int propertyId)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            if (apiResource == null)
            {
                return NotFound();
            }
            var apiProperty = await _context.ApiProperties.FirstOrDefaultAsync(x => x.Id == propertyId && x.ApiResourceId == apiResource.Id);
            if (apiProperty == null)
            {
                return NotFound();
            }
            _context.ApiProperties.Remove(apiProperty);
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