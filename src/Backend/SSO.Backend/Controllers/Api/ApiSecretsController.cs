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
        #region ApiSecrets
        //Get api Secrets for api with id
        [HttpGet("{id}/apiSecrets")]
        public async Task<IActionResult> GetApiSecrets(int id)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            if (apiResource == null)
            {
                return NotFound();
            }
            var query = _context.ApiSecrets.AsQueryable();
            query = query.Where(x => x.ApiResourceId == apiResource.Id);
            var apiSecretsViewModel = query.Select(x => new ApiSecretsViewModel()
            {
                Id = x.Id,
                Type = x.Type,
                Value = x.Value,
                Description = x.Description,
                Created = x.Created,
                Expiration = x.Expiration,
                ApiResourceId = x.ApiResourceId
            });

            return Ok(apiSecretsViewModel);
        }

        //Post new api secret for api resource with id
        [HttpPost("{id}/apiSecrets")]
        public async Task<IActionResult> PostApiSecret(int id, [FromBody]ApiSecretRequest request)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);           
            var apiSecret = new ApiSecret()
            {
                Description = request.Description,
                Type = request.Type,
                Value = request.Value,
                Expiration = request.Expiration,
                Created = request.Created,
                ApiResourceId = request.ApiResourceId
            };
            _context.ApiSecrets.Add(apiSecret);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }

        //Delete api secret for api resource with api id
        [HttpDelete("{id}/apiSecrets/{secretId}")]
        public async Task<IActionResult> DeleteApiSecret(int id, int secretId)
        {
            var apiResource = await _context.ApiResources.FirstOrDefaultAsync(x => x.Id == id);
            if (apiResource == null)
            {
                return NotFound();
            }
            var apiSecret = await _context.ApiSecrets.FirstOrDefaultAsync(x => x.Id == secretId && x.ApiResourceId == apiResource.Id);
            if (apiSecret == null)
            {
                return NotFound();
            }
            _context.ApiSecrets.Remove(apiSecret);
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