using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Services.CreateModel.Client;
using SSO.Services.ViewModel;
using SSO.Services.ViewModel.Client;

namespace SSO.Backend.Controllers
{   
    
    public partial class ClientsController
    {
        //Lấy thông tin client redirect uri để xem hoặc cập nhật
        [HttpGet("{clientId}/redirectUris")]
        public async Task<IActionResult> GetClientRedirectUri(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if(client == null)
            {
                return NotFound();
            }
            var query = _context.ClientRedirectUris.AsQueryable();
            query = query.Where(x => x.ClientId == client.Id);
            var clientRedirectUriViewModel = query.Select(x => new ClientRedirectUriViewModel() {
                Id = x.Id,
                RedirectUri = x.RedirectUri,
                ClientId = x.ClientId
            }); 
            
            return Ok(clientRedirectUriViewModel);
        }

        [HttpPost("{clientId}/redirectUris")]
        public async Task<IActionResult> PostClientRedirectUri(string clientId, [FromBody]ClientRedirectUriRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            var clientRedirectUriRequest = new ClientRedirectUri()
            {
                RedirectUri = request.RedirectUri,
                ClientId = client.Id
            };
            _context.ClientRedirectUris.Add(clientRedirectUriRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete("{clientId}/redirectUris/{id}")]
        public async Task<IActionResult> DeleteClientRedirectUri(string clientId, int id)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if(client == null)
            {
                return NotFound();
            }
            var clientRedirectUri = await _context.ClientRedirectUris.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == client.Id);
            if(clientRedirectUri == null)
            {
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
    }
}