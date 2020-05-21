using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Services.RequestModel.Client;
using SSO.Services.ViewModel.Client;

namespace SSO.Backend.Controllers.Client
{
    public partial class ClientsController
    {
        #region ClientClaim
        //Get Claim for client with client id
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
        //Post new claim for client with client id
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
        //Delete Claim for client with client id
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
        #endregion
    }
}