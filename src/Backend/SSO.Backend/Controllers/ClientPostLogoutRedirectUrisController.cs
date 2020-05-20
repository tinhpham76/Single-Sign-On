using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Data;
using SSO.Services.CreateModel.Client;

namespace SSO.Backend.Controllers
{

    public partial class ClientsController
    {
        
        [HttpPost("{clientId}/postLogoutRedirectUris")]
        public async Task<IActionResult> PostClientPostLogoutRedirectUri(string clientId, [FromBody]ClientPostLogoutRedirectUriRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            var clientPostLogoutRedirectUriRequest = new ClientPostLogoutRedirectUri()
            {
                PostLogoutRedirectUri = request.PostLogoutRedirectUri,
                ClientId = client.Id
            };
            _context.ClientPostLogoutRedirectUris.Add(clientPostLogoutRedirectUriRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}