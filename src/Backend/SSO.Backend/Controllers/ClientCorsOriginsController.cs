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
    }
}