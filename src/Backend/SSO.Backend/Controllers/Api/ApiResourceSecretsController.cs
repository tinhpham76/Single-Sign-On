using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Authorization;
using SSO.Backend.Constants;
using SSO.Services.RequestModel.Api;
using SSO.Services.ViewModel.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Api
{
    public partial class ApiResourcesController
    {
        #region Api Secrets
        //Get api resource secrets
        [HttpGet("{apiResourceName}/apiResourceSecrets")]
        public async Task<IActionResult> GetApiResourceSecrets(string apiResourceName)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            var query = _context.ApiResourceSecrets.Where(x => x.ApiResourceId.Equals(apiResource.Id));
            var apiResourceSecrets = await query.Select(x => new ApiResourceSecretsViewModel()
            {
                Id = x.Id,
                Value = x.Value,
                Type = x.Type,
                Expiration = x.Expiration,
                Description = x.Description
            }).ToListAsync();

            return Ok(apiResourceSecrets);
        }

        //Post api resource secret
        [HttpPost("{apiResourceName}/apiResourceSecrets")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PostApiResourceSecret(string apiResourceName, [FromBody] ApiResourceSecretRequest request)
        {
            //Check Api Resource
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            //If api resource not null, Check api Secret
            if (apiResource != null)
            {
                if (request.HashType == "Sha256")
                {
                    var apiResourceSecretRequest = new IdentityServer4.EntityFramework.Entities.ApiResourceSecret()
                    {
                        Type = request.Type,
                        Value = request.Value.ToSha256(),
                        Description = request.Description,
                        ApiResourceId = apiResource.Id,
                        Expiration = DateTime.Parse(request.Expiration),
                        Created = DateTime.UtcNow
                    };
                    _context.ApiResourceSecrets.Add(apiResourceSecretRequest);
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
                else if (request.HashType == "Sha512")
                {
                    var apiResourceSecretRequest = new IdentityServer4.EntityFramework.Entities.ApiResourceSecret()
                    {
                        Type = request.Type,
                        Value = request.Value.ToSha256(),
                        Description = request.Description,
                        ApiResourceId = apiResource.Id,
                        Expiration = DateTime.Parse(request.Expiration),
                        Created = DateTime.UtcNow
                    };
                    _context.ApiResourceSecrets.Add(apiResourceSecretRequest);
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

        //Delete api resource secret
        [HttpDelete("{apiResourceName}/apiResourceSecrets/{secretId}")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> DeleteApiResourceSecret(string apiResourceName, int secretId)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            if (apiResource == null)
                return NotFound();
            var apiSecret = await _context.ApiResourceSecrets.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id && x.Id == secretId);
            if (apiSecret == null)
                return NotFound();
            _context.ApiResourceSecrets.Remove(apiSecret);
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
