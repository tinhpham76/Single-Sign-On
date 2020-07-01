using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Authorization;
using SSO.Backend.Constants;
using SSO.Services.RequestModel.Api;
using SSO.Services.ViewModel.Api;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Api
{
    public partial class ApiResourcesController
    {
        #region Api Resource Properties
        //Get api resource properties
        [HttpGet("{apiResourceName}/resourceProperties")]
        [ClaimRequirement(PermissionCode.SSO_VIEW)]
        public async Task<IActionResult> GetApiResourceProperties(string apiResourceName)
        {

            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            if (apiResource == null)
                return NotFound();
            var query = _context.ApiResourceProperties.Where(x => x.ApiResourceId.Equals(apiResource.Id));
            var apiResourceProperties = await query.Select(x => new ApiResourcePropertitesViewModel()
            {
                Id = x.Id,
                Key = x.Key,
                Value = x.Value
            }).ToListAsync();

            return Ok(apiResourceProperties);
        }

        //Post api resource property
        [HttpPost("{apiResourceName}/resourceProperties")]
        [ClaimRequirement(PermissionCode.SSO_CREATE)]
        public async Task<IActionResult> PostApiResourceProperty(string apiResourceName, [FromBody] ApiResourcePropertyRequest request)
        {
            //Check Api Resource
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            //If api resource not null, Check api claims
            if (apiResource != null)
            {
                var apiResourceProperty = await _context.ApiResourceProperties.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id);
                //If api claim is null, add claim for client
                if (apiResourceProperty == null)
                {
                    var apiResourcePropertyRequest = new IdentityServer4.EntityFramework.Entities.ApiResourceProperty()
                    {
                        Key = request.Key,
                        Value = request.Value,
                        ApiResourceId = apiResource.Id
                    };
                    _context.ApiResourceProperties.Add(apiResourcePropertyRequest);
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
                else if (apiResourceProperty != null)
                {
                    if (apiResourceProperty.Key == request.Key)
                        return BadRequest($"Api resource property key {request.Key} already exist");

                    var apiResourcePropertyRequest = new IdentityServer4.EntityFramework.Entities.ApiResourceProperty()
                    {
                        Key = request.Key,
                        Value = request.Value,
                        ApiResourceId = apiResource.Id
                    };
                    _context.ApiResourceProperties.Add(apiResourcePropertyRequest);
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

        //Delete api resource property

        [HttpDelete("{apiResourceName}/resourceProperties/{propertyKey}")]
        [ClaimRequirement(PermissionCode.SSO_DELETE)]
        public async Task<IActionResult> DeleteApiResourceProperty(string apiResourceName, string propertyKey)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            if (apiResource == null)
                return NotFound();
            var apiResourceProperty = await _context.ApiResourceProperties.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id && x.Key == propertyKey);
            if (apiResourceProperty == null)
                return NotFound();
            _context.ApiResourceProperties.Remove(apiResourceProperty);
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
