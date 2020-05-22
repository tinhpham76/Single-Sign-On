using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Services.RequestModel.Client;
using SSO.Services.ViewModel.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Client
{
    public partial class ClientsController
    {
        #region ClientClaim
        //Get Claim for client with client id
        [HttpGet("{clientId}/claims")]
        public async Task<IActionResult> GetClientClaims(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var query = _context.ClientClaims.AsQueryable();
            query = query.Where(x => x.ClientId == client.Id);
            var clientClaimsViewModels = query.Select(x => new ClientClaimsViewModel()
            {
                Id = x.Id,
                Type = x.Type,
                Value = x.Value,
                ClientId = x.ClientId
            });

            return Ok(clientClaimsViewModels);
        }
        //Post new claim for client with client id
        [HttpPost("{clientId}/claims")]
        public async Task<IActionResult> PostClientClaim(string clientId, [FromBody]ClientClaimRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            client.Updated = DateTime.UtcNow;
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
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest();
        }
        //Delete Claim for client with client id
        [HttpDelete("{clientId}/claims/{id}")]
        public async Task<IActionResult> DeleteClientClaim(string clientId, int id)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            client.Updated = DateTime.UtcNow;
            var clientClaim = await _context.ClientClaims.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == client.Id);
            if (clientClaim == null)
            {
                return NotFound();
            }
            _context.ClientClaims.Remove(clientClaim);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        #endregion
    }
}