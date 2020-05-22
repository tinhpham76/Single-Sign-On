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
        #region ClientScopes
        //Get Scope for client with client id
        [HttpGet("{clientId}/scopes")]
        public async Task<IActionResult> GetClientScopes(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var query = _context.ClientScopes.AsQueryable();
            query = query.Where(x => x.ClientId == client.Id);
            var clientScopesViewModel = query.Select(x => new ClientScopesViewModel()
            {
                Id = x.Id,
                Scope = x.Scope,
                ClientId = x.ClientId
            });

            return Ok(clientScopesViewModel);
        }

        //Post new Scope for client with client id
        [HttpPost("{clientId}/scopes")]
        public async Task<IActionResult> PostClientScope(string clientId, [FromBody]ClientScopeRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            client.Updated = DateTime.UtcNow;
            var clientScopeRequest = new ClientScope()
            {
                Scope = request.Scope,
                ClientId = client.Id
            };
            _context.ClientScopes.Add(clientScopeRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest();
        }

        //Delete client for client with client id
        [HttpDelete("{clientId}/scopes/{id}")]
        public async Task<IActionResult> DeleteClientScope(string clientId, int id)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            client.Updated = DateTime.UtcNow;
            var clientScope = await _context.ClientScopes.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == client.Id);
            if (clientScope == null)
            {
                return NotFound();
            }
            _context.ClientScopes.Remove(clientScope);
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