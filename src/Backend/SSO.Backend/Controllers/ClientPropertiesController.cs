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
        [HttpPost("{clientId}/properties")]
        public async Task<IActionResult> PostClientPropertie(string clientId, [FromBody]ClientPropertyRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            var clientPropertyRequest = new ClientProperty()
            {
                Key = request.Key,
                Value = request.Value,
                ClientId = client.Id
            };
            _context.ClientProperties.Add(clientPropertyRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }

}