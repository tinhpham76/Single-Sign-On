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
        

        [HttpPost("{clientId}/idPRestrictions")]
        public async Task<IActionResult> PostClientIdPRestriction(string clientId, [FromBody]ClientIdPRestrictionRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            var clientIdPRestriction = await _context.ClientIdPRestrictions.FirstOrDefaultAsync(x => x.ClientId == client.Id);
            var clientIdPRestrictionRequest = new ClientIdPRestriction()
            {
                Provider = request.Provider,
                ClientId = client.Id
            };
            _context.ClientIdPRestrictions.Add(clientIdPRestrictionRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}