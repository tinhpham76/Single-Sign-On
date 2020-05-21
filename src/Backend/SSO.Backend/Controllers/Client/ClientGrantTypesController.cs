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

namespace SSO.Backend.Controllers.Client
{

    public partial class ClientsController
    {
        #region ClientGrantType
        //Get GrantType for client with client id
        [HttpGet("{clientId}/grantTypes")]
        public async Task<IActionResult> GetClientGrantType(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var query = _context.ClientGrantTypes.AsQueryable();
            query = query.Where(x => x.ClientId == client.Id);
            var clientGrantTypeViewModels = query.Select(x => new ClientGrantTypeViewModel()
            {
                Id = x.Id,
                GrantType = x.GrantType,
                ClientId = x.ClientId
            });

            return Ok(clientGrantTypeViewModels);
        }

        //Post new GrantType for client with client id
        [HttpPost("{clientId}/grantTypes")]
        public async Task<IActionResult> PostClientGrantType(string clientId, [FromBody]ClientGrantTypeRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            //var clientGrantType = await _context.ClientGrantTypes.FirstOrDefaultAsync(x => x.ClientId == client.Id);
            var clientGrantTypeRequest = new ClientGrantType()
            {
                GrantType = request.GrantType,
                ClientId = client.Id
            };
            _context.ClientGrantTypes.Add(clientGrantTypeRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }

        //Delete GrantTye for client with client id
        [HttpDelete("{clientId}/grantTypes/{id}")]
        public async Task<IActionResult> DeleteGrantType(string clientId, int id)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var clientGrantType = await _context.ClientGrantTypes.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == client.Id);
            if (clientGrantType == null)
            {
                return NotFound();
            }
            _context.ClientGrantTypes.Remove(clientGrantType);
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