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
        #region ClientProperties
        //Get Properties for client with client id
        [HttpGet("{clientId}/properties")]
        public async Task<IActionResult> GetClientProperties(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var query = _context.ClientProperties.AsQueryable();
            query = query.Where(x => x.ClientId == client.Id);
            var clientPropertiesViewModel = query.Select(x => new ClientPropertiesViewModel()
            {
                Id = x.Id,
                Key = x.Key,
                Value = x.Value,
                ClientId = x.ClientId
            });

            return Ok(clientPropertiesViewModel);
        }

        //Post new Properties for client with client id
        [HttpPost("{clientId}/properties")]
        public async Task<IActionResult> PostClientProperty(string clientId, [FromBody]ClientPropertyRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            client.Updated = DateTime.UtcNow;
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
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest();
        }

        //Delete Properties for client with client id
        [HttpDelete("{clientId}/properties/{id}")]
        public async Task<IActionResult> DeleteClientProperty(string clientId, int id)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            client.Updated = DateTime.UtcNow;
            var clientProperty = await _context.ClientProperties.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == client.Id);
            if (clientProperty == null)
            {
                return NotFound();
            }
            _context.ClientProperties.Remove(clientProperty);
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