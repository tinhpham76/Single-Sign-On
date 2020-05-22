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
        #region ClientPostLogoutRedirectUris
        //Get Logout Redirect Uri for client with client id
        [HttpGet("{clientId}/postLogoutRedirectUris")]
        public async Task<IActionResult> GetPostLogoutRedirectUris(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var query = _context.ClientPostLogoutRedirectUris.AsQueryable();
            query = query.Where(x => x.ClientId == client.Id);
            var clientPostLogoutRedirectUrisViewModels = query.Select(x => new ClientPostLogoutRedirectUrisViewModel()
            {
                Id = x.Id,
                PostLogoutRedirectUri = x.PostLogoutRedirectUri,
                ClientId = x.ClientId
            });

            return Ok(clientPostLogoutRedirectUrisViewModels);
        }

        //Post new Logout Redirect Uri for client with client id
        [HttpPost("{clientId}/postLogoutRedirectUris")]
        public async Task<IActionResult> PostClientPostLogoutRedirectUri(string clientId, [FromBody]ClientPostLogoutRedirectUriRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            client.Updated = DateTime.UtcNow;
            var clientPostLogoutRedirectUriRequest = new ClientPostLogoutRedirectUri()
            {
                PostLogoutRedirectUri = request.PostLogoutRedirectUri,
                ClientId = client.Id
            };
            _context.ClientPostLogoutRedirectUris.Add(clientPostLogoutRedirectUriRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest();
        }

        //Delete Logout Redirect Uri for client with client id
        [HttpDelete("{clientId}/postLogoutRedirectUris/{id}")]
        public async Task<IActionResult> DeletePostLogoutRedirectUris(string clientId, int id)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            client.Updated = DateTime.UtcNow;
            var clientPostLogoutRedirectUri = await _context.ClientPostLogoutRedirectUris.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == client.Id);
            if (clientPostLogoutRedirectUri == null)
            {
                return NotFound();
            }
            _context.ClientPostLogoutRedirectUris.Remove(clientPostLogoutRedirectUri);
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