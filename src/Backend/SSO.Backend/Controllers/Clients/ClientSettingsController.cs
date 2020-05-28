using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Authorization;
using SSO.Backend.Constants;
using SSO.Services.RequestModel.Client;
using SSO.Services.ViewModel.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers.Clients
{
    public partial class ClientsController
    {
        #region Client Setting
        //Get setting infor client for edit
        [HttpGet("{clientId}/settings")]
        public async Task<IActionResult> GetClientSetting(string clientId)
        {
            var client = await _clientStore.FindClientByIdAsync(clientId);
            if (client == null)
                return NotFound();
            var clientSettingViewModel = new ClientSettingViewModel()
            {
                Enabled = client.Enabled,
                AllowedScopes = client.AllowedScopes.Select(x => x.ToString()).ToList(),
                RedirectUris = client.RedirectUris.Select(x => x.ToString()).ToList(),
                AllowedGrantTypes = client.AllowedGrantTypes.Select(x => x.ToString()).ToList(),
                RequireConsent = client.RequireConsent,
                AllowRememberConsent = client.AllowRememberConsent,
                AllowOfflineAccess = client.AllowOfflineAccess,
                RequireClientSecret = client.RequireClientSecret,
                ProtocolType = client.ProtocolType,
                RequirePkce = client.RequirePkce,
                AllowPlainTextPkce = client.AllowPlainTextPkce,
                AllowAccessTokensViaBrowser = client.AllowAccessTokensViaBrowser
            };
            return Ok(clientSettingViewModel);
        }

        //Edit setting infor
        [HttpPut("{clientId}/settings")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PutClientBasic(string clientId, [FromBody]ClientSettingRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return NotFound();
            //Table Clients
            client.Enabled = request.Enabled;
            client.RequireConsent = request.RequireConsent;
            client.AllowRememberConsent = request.AllowRememberConsent;
            client.AllowOfflineAccess = request.AllowOfflineAccess;
            client.RequireClientSecret = request.RequireClientSecret;
            client.ProtocolType = request.ProtocolType;
            client.RequirePkce = request.RequirePkce;
            client.AllowPlainTextPkce = request.AllowPlainTextPkce;
            client.AllowAccessTokensViaBrowser = request.AllowAccessTokensViaBrowser;
            client.Updated = DateTime.UtcNow;

            _configurationDbContext.Update(client);
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }

        //Post Client Scope for client
        [HttpPost("{clientId}/settings/scopes")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PostClientScopes(string clientId, [FromBody]ClientScopeRequest request)
        {
            //Check Client
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            //If client not null, Check Client Scope
            if (client != null)
            {
                var clientScope = await _context.ClientScopes.FirstOrDefaultAsync(x => x.ClientId == client.Id);
                //If Client Scope is null, add Scope for client
                if (clientScope == null)
                {
                    var clientScopeRequest = new IdentityServer4.EntityFramework.Entities.ClientScope()
                    {
                        Scope = request.Scope,
                        ClientId = client.Id
                    };
                    _context.ClientScopes.Add(clientScopeRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        client.Updated = DateTime.UtcNow;
                        _configurationDbContext.Clients.Update(client);
                        await _configurationDbContext.SaveChangesAsync();
                        return Ok();
                    }
                    return BadRequest();
                }
                // If Client Scope not null, Check Client Scope on table with request Scope
                else if (clientScope != null)
                {
                    if (clientScope.Scope == request.Scope)
                        return BadRequest($"Client Scope {request.Scope} already exist");

                    var clientScopeRequest = new IdentityServer4.EntityFramework.Entities.ClientScope()
                    {
                        Scope = request.Scope,
                        ClientId = client.Id
                    };
                    _context.ClientScopes.Add(clientScopeRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        client.Updated = DateTime.UtcNow;
                        _configurationDbContext.Clients.Update(client);
                        await _configurationDbContext.SaveChangesAsync();
                        return Ok();
                    }
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        //Delete Client Scope 
        [HttpDelete("{clientId}/settings/scopes/{scopeId}")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> DeleteClientScope(string clientId, int scopeId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return BadRequest();
            var clientScope = await _context.ClientScopes.FirstOrDefaultAsync(x => x.ClientId == client.Id && x.Id == scopeId);
            if (clientScope == null)
                return NotFound();
            _context.ClientScopes.Remove(clientScope);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                client.Updated = DateTime.UtcNow;
                _configurationDbContext.Clients.Update(client);
                await _configurationDbContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        //Post Client RedirectUris for client
        [HttpPost("{clientId}/settings/redirectUris")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PostClientRedirectUri(string clientId, [FromBody]ClientRedirectUriRequest request)
        {
            //Check Client
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            //If client not null, Check Client RedirectUris
            if (client != null)
            {
                var clientRedirectUri = await _context.ClientRedirectUris.FirstOrDefaultAsync(x => x.ClientId == client.Id);
                //If Client RedirectUris is null, add RedirectUris for client
                if (clientRedirectUri == null)
                {
                    var clientRedirectUriRequest = new IdentityServer4.EntityFramework.Entities.ClientRedirectUri()
                    {
                        RedirectUri = request.RedirectUri,
                        ClientId = client.Id
                    };
                    _context.ClientRedirectUris.Add(clientRedirectUriRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        client.Updated = DateTime.UtcNow;
                        _configurationDbContext.Clients.Update(client);
                        await _configurationDbContext.SaveChangesAsync();
                        return Ok();
                    }
                    return BadRequest();
                }
                // If Client RedirectUris not null, Check Client RedirectUris on table with request RedirectUris
                else if (clientRedirectUri != null)
                {
                    if (clientRedirectUri.RedirectUri == request.RedirectUri)
                        return BadRequest($"Client RedirectUri {request.RedirectUri} already exist");

                    var clientRedirectUriRequest = new IdentityServer4.EntityFramework.Entities.ClientRedirectUri()
                    {
                        RedirectUri = request.RedirectUri,
                        ClientId = client.Id
                    };
                    _context.ClientRedirectUris.Add(clientRedirectUriRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        client.Updated = DateTime.UtcNow;
                        _configurationDbContext.Clients.Update(client);
                        await _configurationDbContext.SaveChangesAsync();
                        return Ok();
                    }
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        //Delete Client RedirectUris 
        [HttpDelete("{clientId}/settings/redirectUris/{redirectUriId}")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> DeleteClienRedirectUri(string clientId, int redirectUriId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return BadRequest();
            var clienRedirectUri = await _context.ClientRedirectUris.FirstOrDefaultAsync(x => x.ClientId == client.Id && x.Id == redirectUriId);
            if (clienRedirectUri == null)
                return NotFound();
            _context.ClientRedirectUris.Remove(clienRedirectUri);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                client.Updated = DateTime.UtcNow;
                _configurationDbContext.Clients.Update(client);
                await _configurationDbContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        //Post Client GrantType for client
        [HttpPost("{clientId}/settings/grantTypes")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PostClientGrantType(string clientId, [FromBody]ClientGrantTypeRequest request)
        {
            //Check Client
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            //If client not null, Check Client GrantType
            if (client != null)
            {
                var clientGrantType = await _context.ClientGrantTypes.FirstOrDefaultAsync(x => x.ClientId == client.Id);
                //If Client GrantType is null, add GrantType for client
                if (clientGrantType == null)
                {
                    var clientGrantTypeRequest = new IdentityServer4.EntityFramework.Entities.ClientGrantType()
                    {
                        GrantType = request.GrantType,
                        ClientId = client.Id
                    };
                    _context.ClientGrantTypes.Add(clientGrantTypeRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        client.Updated = DateTime.UtcNow;
                        _configurationDbContext.Clients.Update(client);
                        await _configurationDbContext.SaveChangesAsync();
                        return Ok();
                    }
                    return BadRequest();
                }
                // If Client GrantType not null, Check Client GrantType on table with request GrantType
                else if (clientGrantType != null)
                {
                    if (clientGrantType.GrantType == request.GrantType)
                        return BadRequest($"Client GrantType {request.GrantType} already exist");

                    var clientGrantTypeRequest = new IdentityServer4.EntityFramework.Entities.ClientGrantType()
                    {
                        GrantType = request.GrantType,
                        ClientId = client.Id
                    };
                    _context.ClientGrantTypes.Add(clientGrantTypeRequest);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        client.Updated = DateTime.UtcNow;
                        _configurationDbContext.Clients.Update(client);
                        await _configurationDbContext.SaveChangesAsync();
                        return Ok();
                    }
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        //Delete Client GrantType 
        [HttpDelete("{clientId}/settings/grantTypes/{grantTypeId}")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> DeleteClientGrantType(string clientId, int grantTypeId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return BadRequest();
            var clientGrantType = await _context.ClientGrantTypes.FirstOrDefaultAsync(x => x.ClientId == client.Id && x.Id == grantTypeId);
            if (clientGrantType == null)
                return NotFound();
            _context.ClientGrantTypes.Remove(clientGrantType);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                client.Updated = DateTime.UtcNow;
                _configurationDbContext.Clients.Update(client);
                await _configurationDbContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        #endregion
    }
}
