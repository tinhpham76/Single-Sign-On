﻿using System;
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

namespace SSO.Backend.Controllers
{

    public partial class ClientsController
    {
        [HttpGet("{clientId}/postLogoutRedirectUris")]
        public async Task<IActionResult> GetPostLogoutRedirectUri(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var query = _context.ClientPostLogoutRedirectUris.AsQueryable();
            query = query.Where(x => x.ClientId == client.Id);
            var clientPostLogoutRedirectUriViewModels = query.Select(x => new ClientPostLogoutRedirectUriViewModel()
            {
                Id = x.Id,
                PostLogoutRedirectUri = x.PostLogoutRedirectUri,
                ClientId = x.ClientId
            });

            return Ok(clientPostLogoutRedirectUriViewModels);
        }


        [HttpPost("{clientId}/postLogoutRedirectUris")]
        public async Task<IActionResult> PostClientPostLogoutRedirectUri(string clientId, [FromBody]ClientPostLogoutRedirectUriRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            var clientPostLogoutRedirectUriRequest = new ClientPostLogoutRedirectUri()
            {
                PostLogoutRedirectUri = request.PostLogoutRedirectUri,
                ClientId = client.Id
            };
            _context.ClientPostLogoutRedirectUris.Add(clientPostLogoutRedirectUriRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete("{clientId}/postLogoutRedirectUris/{id}")]
        public async Task<IActionResult> DeletePostLogoutRedirectUris(string clientId, int id)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var clientPostLogoutRedirectUri = await _context.ClientPostLogoutRedirectUris.FirstOrDefaultAsync(x => x.Id == id && x.ClientId == client.Id);
            if (clientPostLogoutRedirectUri == null)
            {
                return NotFound();
            }
            _context.ClientPostLogoutRedirectUris.Remove(clientPostLogoutRedirectUri);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}