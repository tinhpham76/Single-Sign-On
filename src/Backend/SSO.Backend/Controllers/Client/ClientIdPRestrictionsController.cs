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

        #region ClientIdPRestrictions
        //Get IdPRestrictions for client with client id
        [HttpGet("{clientId}/idPRestrictions")]
        public async Task<IActionResult> GetClientIdPRestrictions(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var query = _context.ClientIdPRestrictions.AsQueryable();
            query = query.Where(x => x.ClientId == client.Id);
            var clientIdPRestrictionsViewModel = query.Select(x => new ClientIdPRestrictionsViewModel()
            {
                Id = x.Id,
                Provider = x.Provider,
                ClientId = x.ClientId
            });

            return Ok(clientIdPRestrictionsViewModel);
        }

        //Post new IdPRestrictions for client with client id
        [HttpPost("{clientId}/idPRestrictions")]
        public async Task<IActionResult> PostClientIdPRestriction(string clientId, [FromBody]ClientIdPRestrictionRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            client.Updated = DateTime.UtcNow;
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
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest();
        }

        //Delete IdPRestrictions for client with client id
        [HttpDelete("{clientId}/idPRestrictions/{id}")]
        public async Task<IActionResult> DeleteClientIdPRestriction(string clientId, int id)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            client.Updated = DateTime.UtcNow;
            var clientIdPRestriction = await _context.ClientIdPRestrictions.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == client.Id);
            if (clientIdPRestriction == null)
            {
                return NotFound();
            }
            _context.ClientIdPRestrictions.Remove(clientIdPRestriction);
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