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
        [HttpPost("{clientId}/scopes")]
        public async Task<IActionResult> PostClientScope(string clientId, [FromBody]ClientScopeRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            var clientScopeRequest = new ClientScope()
            {
                Scope = request.Scope,
                ClientId = client.Id
            };
            _context.ClientScopes.Add(clientScopeRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}