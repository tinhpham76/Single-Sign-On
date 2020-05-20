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
        

        [HttpPost("{clientId}/grantTypes")]
        public async Task<IActionResult> PostClientGrantType(string clientId, [FromBody]ClientGrantTypeRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            var clientGrantType = await _context.ClientGrantTypes.FirstOrDefaultAsync(x => x.ClientId == client.Id);
            var clientGrantTypeRequest = new ClientGrantType()
            {
                GrantType = request.GrantType,
                ClientId = client.Id
            };
            _context.ClientGrantTypes.Add(clientGrantTypeRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}