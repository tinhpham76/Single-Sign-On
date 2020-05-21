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
        #region ClientScopes
        //Get Scope for client with client id
        [HttpGet("{clientId}/scopes")]
        public async Task<IActionResult> GetClientScope(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var query = _context.ClientScopes.AsQueryable();
            query = query.Where(x => x.ClientId == client.Id);
            var clientScopeViewModel = query.Select(x => new ClientScopeViewModel()
            {
                Id = x.Id,
                Scope = x.Scope,
                ClientId = x.ClientId
            });

            return Ok(clientScopeViewModel);
        }

        //Post new Scope for client with client id
        [HttpPost("{clientId}/scopes")]
        public async Task<IActionResult> PostClientScope(string clientId, [FromBody]ClientScopeRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            var clientScopeRequest = new ClientScope()
            {
                Scope = request.Scope,
                ClientId = client.Id
            };
            _context.ClientScopes.Add(clientScopeRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
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
            var clientScope = await _context.ClientScopes.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == client.Id);
            if (clientScope == null)
            {
                return NotFound();
            }
            _context.ClientScopes.Remove(clientScope);
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