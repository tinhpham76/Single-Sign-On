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
using SSO.Services.ViewModel.Client;

namespace SSO.Backend.Controllers
{
    
    public partial class ClientsController
    {
        [HttpGet("{clientId}/idPRestrictions")]
        public async Task<IActionResult> GetClientIdPRestriction(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var query = _context.ClientIdPRestrictions.AsQueryable();
            query = query.Where(x => x.ClientId == client.Id);
            var clientIdPRestrictionViewModel = query.Select(x => new ClientIdPRestrictionViewModel()
            {
                Id = x.Id,
                Provider= x.Provider,
                ClientId = x.ClientId
            });

            return Ok(clientIdPRestrictionViewModel);
        }

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

        [HttpDelete("{clientId}/idPRestrictions/{id}")]
        public async Task<IActionResult> DeleteClientIdPRestriction(string clientId, int id)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var clientIdPRestriction = await _context.ClientIdPRestrictions.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == client.Id);
            if (clientIdPRestriction == null)
            {
                return NotFound();
            }
            _context.ClientIdPRestrictions.Remove(clientIdPRestriction);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}