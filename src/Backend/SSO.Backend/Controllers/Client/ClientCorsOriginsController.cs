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
        #region ClientCorsOrigins
        //Get Origins for client with client id
        [HttpGet("{clientId}/corsOrigins")]
        public async Task<IActionResult> GetClientCorsOrigins(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var query = _context.ClientCorsOrigins.AsQueryable();
            query = query.Where(x => x.ClientId == client.Id);
            var clientCorsOriginsViewModels = query.Select(x => new ClientCorsOriginsViewModel()
            {
                Id = x.Id,
                Origin = x.Origin,
                ClientId = x.ClientId
            });

            return Ok(clientCorsOriginsViewModels);
        }

        //Post new Origins for client with client id
        [HttpPost("{clientId}/corsOrigins")]
        public async Task<IActionResult> PostClientCorsOrigin(string clientId, [FromBody]ClientCorsOriginRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            var clientCorsOrigins = await _context.ClientCorsOrigins.FirstOrDefaultAsync(x => x.ClientId == client.Id);
            var clientCorsOriginsRequest = new ClientCorsOrigin()
            {
                Origin = request.Origin,
                ClientId = client.Id
            };
            _context.ClientCorsOrigins.Add(clientCorsOriginsRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }

        //Delete Origins for client with client id
        [HttpDelete("{clientId}/corsOrigins/{id}")]
        public async Task<IActionResult> DeleteClientCorsOrigin(string clientId, int id)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var clientCorsOrigins = await _context.ClientCorsOrigins.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == client.Id);
            if (clientCorsOrigins == null)
            {
                return NotFound();
            }
            _context.ClientCorsOrigins.Remove(clientCorsOrigins);
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