using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Services.CreateModel.Client;

namespace SSO.Backend.Controllers
{
    
    public partial class ClientsController
    {
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

    }
}