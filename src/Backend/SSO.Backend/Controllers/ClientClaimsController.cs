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
        [HttpGet("{clientId}/claims")]
        public async Task<IActionResult> GetClientClaim(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var query = _context.ClientClaims.AsQueryable();
            query = query.Where(x => x.ClientId == client.Id);
            var clientClaimViewModels = query.Select(x => new ClientClaimViewModel()
            {
                Id = x.Id,
                Type = x.Type,
                Value = x.Value,
                ClientId = x.ClientId
            });

            return Ok(clientClaimViewModels);
        }

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

        [HttpDelete("{clientId}/claims/{id}")]
        public async Task<IActionResult> DeleteClientClaim(string clientId, int id)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var clientClaim = await _context.ClientClaims.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == client.Id);
            if (clientClaim == null)
            {
                return NotFound();
            }
            _context.ClientClaims.Remove(clientClaim); 
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}