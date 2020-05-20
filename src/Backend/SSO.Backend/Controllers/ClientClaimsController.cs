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
        
        [HttpPost("{clientId}/claims")]
        public async Task<IActionResult> PostClientClaim(string clientId, [FromBody]ClientClaimRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            var clientClaimRequest = new ClientClaim()
            {
                Type = request.Type,
                Value = request.Value,
                ClientId = client.Id
            };
            _context.ClientClaims.Add(clientClaimRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}