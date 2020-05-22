using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Services.RequestModel.Client;
using SSO.Services.ViewModel.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Client
{
    public partial class ClientsController
    {
        #region ClientSecrets
        //Get Secrets for client with clien id
        [HttpGet("{clientId}/secrets")]
        public async Task<IActionResult> GetClientSecrets(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var query = _context.ClientSecrets.AsQueryable();
            query = query.Where(x => x.ClientId == client.Id);
            var clientSecretsViewModel = query.Select(x => new ClientSecretsViewModel()
            {
                Id = x.Id,
                Description = x.Description,
                Expiration = x.Expiration,
                Type = x.Type,
                Value = x.Value,
                Created = x.Created,
                ClientId = x.ClientId
            });
            return Ok(clientSecretsViewModel);
        }

        //Post new Client Secrets for client with client id
        [HttpPost("{clientId}/secrets")]
        public async Task<IActionResult> PostClientSecret(string clientId, [FromBody]ClientSecretRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            client.Updated = DateTime.UtcNow;
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
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
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
            client.Updated = DateTime.UtcNow;
            var clientSecret = await _context.ClientSecrets.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == client.Id);
            if (clientSecret == null)
            {
                return NotFound();
            }
            _context.ClientSecrets.Remove(clientSecret);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        #endregion
    }
}