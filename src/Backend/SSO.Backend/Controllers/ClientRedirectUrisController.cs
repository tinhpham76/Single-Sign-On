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
        [HttpPost("{clientId}/redirectUris")]
        public async Task<IActionResult> PostClientRedirectUri(string clientId, [FromBody]ClientRedirectUriRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            var clientRedirectUriRequest = new ClientRedirectUri()
            {
                RedirectUri = request.RedirectUri,
                ClientId = client.Id
            };
            _context.ClientRedirectUris.Add(clientRedirectUriRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}