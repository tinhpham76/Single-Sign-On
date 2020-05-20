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
       

        [HttpPost("{clientId}/corsOrigins")]
        public async Task<IActionResult> PostClientCorsOrigin(string clientId, [FromBody]ClientCorsOriginRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            var clientCorsOrigins = await _context.ClientCorsOrigins.FirstOrDefaultAsync(x => x.ClientId == client.Id);
            var clientCorsOriginsRequest = new ClientCorsOrigin()
            {
                Origin = request.Origin,
                ClientId = client.Id
            };
            _context.ClientCorsOrigins.Add(clientCorsOriginsRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}