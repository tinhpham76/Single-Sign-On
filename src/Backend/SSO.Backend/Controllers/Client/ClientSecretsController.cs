using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Services.CreateModel.Client;
using SSO.Services.ViewModel.Client;

namespace SSO.Backend.Controllers.Client
{
    public partial class ClientsController
    {
        #region ClientSecrets
        //Get Secrets for client with clien id
        [HttpGet("{clientId}/secrets")]
        public async Task<IActionResult> GetClientSecret(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var query = _context.ClientSecrets.AsQueryable();
            query = query.Where(x => x.ClientId == client.Id);
            var clientRedirectUriViewModel = query.Select(x => new ClientSecretViewModel()
            {
                Id = x.Id,
                Description = x.Description,
                Expiration = x.Expiration,
                Type = x.Type,
                Value = x.Value,
                Created = x.Created,
                ClientId = x.ClientId
            });
            return Ok(clientRedirectUriViewModel);
        }

        //Post new Client Secrets for client with client id
        [HttpPost("{clientId}/secrets")]
        public async Task<IActionResult> PostClientSecret(string clientId, [FromBody]ClientSecretRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            var clientSecretRequest = new ClientSecret()
            {
                Description = request.Description,
                Value = request.Value,
                Expiration = request.Expiration,
                Type = request.Type,
                Created = DateTime.UtcNow,
                ClientId = client.Id
            };
            _context.ClientSecrets.Add(clientSecretRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }

        //Delete Client Secret for client with client id
        [HttpDelete("{clientId}/secrets/{id}")]
        public async Task<IActionResult> DeleteClientSecre(string clientId, int id)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var clientSecret = await _context.ClientSecrets.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == client.Id);
            if (clientSecret == null)
            {
                return NotFound();
            }
            _context.ClientSecrets.Remove(clientSecret);
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