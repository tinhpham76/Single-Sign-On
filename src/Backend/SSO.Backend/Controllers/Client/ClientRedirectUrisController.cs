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
        #region ClientRedirectUris
        //Get Redirect Uri for client with client id
        [HttpGet("{clientId}/redirectUris")]
        public async Task<IActionResult> GetClientRedirectUris(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var query = _context.ClientRedirectUris.AsQueryable();
            query = query.Where(x => x.ClientId == client.Id);
            var clientRedirectUrisViewModel = query.Select(x => new ClientRedirectUrisViewModel()
            {
                Id = x.Id,
                RedirectUri = x.RedirectUri,
                ClientId = x.ClientId
            });

            return Ok(clientRedirectUrisViewModel);
        }

        //Post new Redirect Uri for client with client id
        [HttpPost("{clientId}/redirectUris")]
        public async Task<IActionResult> PostClientRedirectUri(string clientId, [FromBody]ClientRedirectUriRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            client.Updated = DateTime.UtcNow;
            var clientRedirectUriRequest = new ClientRedirectUri()
            {
                RedirectUri = request.RedirectUri,
                ClientId = client.Id
            };
            _context.ClientRedirectUris.Add(clientRedirectUriRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest();
        }

        //Delete Redirect Uri for client with client id
        [HttpDelete("{clientId}/redirectUris/{id}")]
        public async Task<IActionResult> DeleteClientRedirectUri(string clientId, int id)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            client.Updated = DateTime.UtcNow;
            var clientRedirectUri = await _context.ClientRedirectUris.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == client.Id);
            if (clientRedirectUri == null)
            {
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
                return NotFound();
            }
            _context.ClientRedirectUris.Remove(clientRedirectUri);
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