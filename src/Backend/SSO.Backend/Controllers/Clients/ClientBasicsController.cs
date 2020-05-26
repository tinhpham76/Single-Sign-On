﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Services.RequestModel.Client;
using SSO.Services.ViewModel.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Clients
{
    public partial class ClientsController
    {
        #region Client Basics
        //Get basic infor client for edit
        [HttpGet("{clientId}/basics")]
        public async Task<IActionResult> GetClientBasic(string clientId)
        {
            var client = await _clientStore.FindClientByIdAsync(clientId);
            if (client == null)
                return NotFound();
            var clientBasicViewModel = new ClientBasicViewModel()
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                ClientUri = client.ClientUri,
                Description = client.Description,
                LogoUri = client.LogoUri,
                AllowedCorsOrigins = client.AllowedCorsOrigins.Select(x => x.ToString()).ToList()

            };
            return Ok(clientBasicViewModel);

        }
        //Edit basic infor
        [HttpPut("{clientId}/basics")]
        public async Task<IActionResult> PutClientBasic(string clientId, [FromBody]ClientBasicRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return NotFound();
            //Table Clients
            client.ClientId = request.ClientId;
            client.ClientName = request.ClientName;
            client.Description = request.Description;
            client.ClientUri = request.ClientUri;
            client.LogoUri = request.LogoUri;
            client.Updated = DateTime.UtcNow;
            _configurationDbContext.Update(client);
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }

        //Post Client Origins for client
        [HttpPost("{clientId}/basics/origins")]
        public async Task<IActionResult> PostClientOrigin(string clientId, [FromBody]ClientOriginRequest request)
        {
            //Check Client
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            //If client not null, Check Client Origin
            if (client != null)
            {
                var clientOrigin = await _context.ClientCorsOrigins.FirstOrDefaultAsync(x => x.ClientId == client.Id);
                //If Client Origin is null, add Origin for client
                if (clientOrigin == null)
                {
                    var clientOriginRequest = new IdentityServer4.EntityFramework.Entities.ClientCorsOrigin()
                    {
                        Origin = request.Origin,
                        ClientId = client.Id
                    };
                    _context.ClientCorsOrigins.Add(clientOriginRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                        return Ok();
                    return BadRequest();
                }
                // If Client Origin not null, Check Client Origin on table with request Origin
                else if (clientOrigin != null)
                {
                    if (clientOrigin.Origin == request.Origin)
                        return BadRequest($"Client Origin {request.Origin} already exist");

                    var clientOriginRequest = new IdentityServer4.EntityFramework.Entities.ClientCorsOrigin()
                    {
                        Origin = request.Origin,
                        ClientId = client.Id
                    };
                    _context.ClientCorsOrigins.Add(clientOriginRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                        return Ok();
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        //Delete Client Origin 
        [HttpDelete("{clientId}/basics/origins/{originId}")]
        public async Task<IActionResult> DeleteClientOrigin(string clientId, int originId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return BadRequest();
            var clientOrigin = await _context.ClientCorsOrigins.FirstOrDefaultAsync(x => x.ClientId == client.Id && x.Id == originId);
            if (clientOrigin == null)
                return NotFound();
            _context.ClientCorsOrigins.Remove(clientOrigin);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }

        #endregion
    }
}