﻿
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Services.RequestModel.Api;
using SSO.Services.ViewModel.Api;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Api
{
    public partial class ApiResourcesController
    {
        #region Api Secret
        //Get api claim
        [HttpGet("{apiResourceName}/apiSecrets")]
        public async Task<IActionResult> GetApiSecrets(string apiResourceName)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            var query = _context.ApiSecrets.Where(x => x.ApiResourceId.Equals(apiResource.Id));
            var apiSecrets = await query.Select(x => new ApiSecretsViewModel()
            {
                Id = x.Id,
                Value = x.Value,
                Type = x.Type,
                Expiration = x.Expiration,
                Description = x.Description
            }).ToListAsync();

            return Ok(apiSecrets);
        }

        //Post api secret
        [HttpPost("{apiResourceName}/apiSecrets")]
        public async Task<IActionResult> PostApiSecret(string apiResourceName, [FromBody]ApiSecretRequest request)
        {
            //Check Api Resource
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            //If api resource not null, Check api Secret
            if (apiResource != null)
            {
                if (request.HasType == "sha256")
                {
                    var apiSecretRequest = new IdentityServer4.EntityFramework.Entities.ApiSecret()
                    {
                        Type = request.Type,
                        Value = request.Value.ToSha256(),
                        Description = request.Description,
                        Expiration = request.Expiration,
                        ApiResourceId = apiResource.Id
                    };
                    _context.ApiSecrets.Add(apiSecretRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                        return Ok();
                    return BadRequest();
                }
                else if (request.HasType == "sha512")
                {
                    var apiSecretRequest = new IdentityServer4.EntityFramework.Entities.ApiSecret()
                    {
                        Type = request.Type,
                        Value = request.Value.ToSha512(),
                        Description = request.Description,
                        Expiration = request.Expiration,
                        ApiResourceId = apiResource.Id
                    };
                    _context.ApiSecrets.Add(apiSecretRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                        return Ok();
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        //Delete api claim
        [HttpDelete("{apiResourceName}/apiSecrets/{secretId}")]
        public async Task<IActionResult> DeleteApiSecret(string apiResourceName, int secretId)
        {
            var apiResource = await _configurationDbContext.ApiResources.FirstOrDefaultAsync(x => x.Name == apiResourceName);
            if (apiResource == null)
                return NotFound();
            var apiSecret = await _context.ApiSecrets.FirstOrDefaultAsync(x => x.ApiResourceId == apiResource.Id && x.Id == secretId);
            if (apiSecret == null)
                return NotFound();
            _context.ApiSecrets.Remove(apiSecret);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();

        }
        #endregion
    }
}