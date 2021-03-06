﻿
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Authorization;
using SSO.Backend.Constants;
using SSO.Backend.Data;
using SSO.Services;
using SSO.Services.RequestModel.Client;
using SSO.Services.ViewModel.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Clients
{
    public partial class ClientsController : BaseController
    {
        #region Clients              
        private readonly IClientStore _clientStore;
        private readonly ApplicationDbContext _context;
        private readonly ConfigurationDbContext _configurationDbContext;
        public ClientsController(ConfigurationDbContext configurationDbContext, IClientStore clientStore, ApplicationDbContext context
            )
        {
            _clientStore = clientStore;
            _context = context;
            _configurationDbContext = configurationDbContext;
        }

        //Show basic infor all client
        [HttpGet("{clientId}")]
        [ClaimRequirement(PermissionCode.SSO_VIEW)]
        public async Task<IActionResult> GetClientDetail(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return NotFound();
            return Ok(client);
        }

        // Find clients with client name or id
        [HttpGet("filter")]
        [ClaimRequirement(PermissionCode.SSO_VIEW)]
        public async Task<IActionResult> GetClientsPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _configurationDbContext.Clients.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.ClientId.Contains(filter) || x.ClientName.Contains(filter));

            }
            var totalReconds = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ClientsQuickView()
                {
                    Id = x.Id,
                    ClientId = x.ClientId,
                    ClientName = x.ClientName,
                    LogoUri = x.LogoUri
                }).ToListAsync();

            var pagination = new Pagination<ClientsQuickView>
            {
                Items = items,
                TotalRecords = totalReconds
            };
            return Ok(pagination);
        }

        //Post basic info client
        [HttpPost]
        [ClaimRequirement(PermissionCode.SSO_CREATE)]
        public async Task<IActionResult> PostClient([FromBody] ClientQuickRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientName == request.ClientName);
            if (client != null)
                return BadRequest($"Client {request.ClientName} already exist!");
            if (request.ClientType == "empty")
            {
                var clientQuickRequest = new Client()
                {
                    ClientId = Guid.NewGuid().ToString(),
                    ClientName = request.ClientName,
                    Description = request.Description,
                    ClientUri = request.ClientUri,
                    LogoUri = request.LogoUri,
                    AllowedGrantTypes = GrantTypes.Implicit
                };
                _configurationDbContext.Add(clientQuickRequest.ToEntity());
            }
            else if (request.ClientType == "web_app_authorization_code"
                || request.ClientType == "spa"
                || request.ClientType == "native")
            {
                var clientQuickRequest = new Client()
                {
                    ClientId = Guid.NewGuid().ToString(),
                    ClientName = request.ClientName,
                    Description = request.Description,
                    ClientUri = request.ClientUri,
                    LogoUri = request.LogoUri,
                    AllowedGrantTypes = GrantTypes.Code,

                };
                _configurationDbContext.Add(clientQuickRequest.ToEntity());
            }
            else if (request.ClientType == "web_app_hybrid")
            {
                var clientQuickRequest = new Client()
                {
                    ClientId = Guid.NewGuid().ToString(),
                    ClientName = request.ClientName,
                    Description = request.Description,
                    ClientUri = request.ClientUri,
                    LogoUri = request.LogoUri,
                    AllowedGrantTypes = GrantTypes.Hybrid,
                };
                _configurationDbContext.Add(clientQuickRequest.ToEntity());
            }
            else if (request.ClientType == "server")
            {
                var clientQuickRequest = new Client()
                {
                    ClientId = Guid.NewGuid().ToString(),
                    ClientName = request.ClientName,
                    Description = request.Description,
                    ClientUri = request.ClientUri,
                    LogoUri = request.LogoUri,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                };
                _configurationDbContext.Add(clientQuickRequest.ToEntity());
            }
            else if (request.ClientType == "device")
            {
                var clientQuickRequest = new Client()
                {
                    ClientId = Guid.NewGuid().ToString(),
                    ClientName = request.ClientName,
                    Description = request.Description,
                    ClientUri = request.ClientUri,
                    LogoUri = request.LogoUri,
                    AllowedGrantTypes = GrantTypes.DeviceFlow,
                };
                _configurationDbContext.Add(clientQuickRequest.ToEntity());
            }
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }

        //Delele client
        [HttpDelete("{clientId}")]
        [ClaimRequirement(PermissionCode.SSO_DELETE)]
        public async Task<IActionResult> DeleteClient(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return NotFound();
            _configurationDbContext.Clients.Remove(client);
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }
        #endregion
    }
}