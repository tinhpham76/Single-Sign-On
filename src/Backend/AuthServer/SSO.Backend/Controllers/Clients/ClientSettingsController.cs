using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Authorization;
using SSO.Backend.Constants;
using SSO.Services.RequestModel.Client;
using SSO.Services.ViewModel.Client;
using System;
using System.Collections.Generic;
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
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return NotFound();
            var allowedScopes = await _context.ClientScopes
                .Where(x => x.ClientId == client.Id)
                .Select(x => x.Scope.ToString()).ToListAsync();
            var redirectUris = await _context.ClientRedirectUris
                .Where(x => x.ClientId == client.Id)
                .Select(x => x.RedirectUri.ToString()).ToListAsync();
            var allowedGrantTypes = await _context.ClientGrantTypes
                .Where(x => x.ClientId == client.Id)
                .Select(x => x.GrantType.ToString()).ToListAsync();
            var clientSettingViewModel = new ClientSettingViewModel()
            {
                Enabled = client.Enabled,
                AllowedScopes = allowedScopes,
                RedirectUris = redirectUris,
                AllowedGrantTypes = allowedGrantTypes,
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

        //Get setting infor client for edit
        [HttpGet("allScopes")]
        public async Task<IActionResult> GetAllScopes()
        {
            var api = await _configurationDbContext.ApiResources
                .Select(x => x.Name.ToString()).ToListAsync();
            var apiScope = await _configurationDbContext.ApiScopes
                .Select(x => x.Name.ToString()).ToListAsync();
            var identity = await _configurationDbContext.IdentityResources
                .Select(x => x.Name.ToString()).ToListAsync();

            var allScope = (api.Concat(identity)).Concat(apiScope);
            return Ok(allScope);
        }

        //Edit setting infor
        [HttpPut("{clientId}/settings")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PutClientBasic(string clientId, [FromBody] ClientSettingRequest request)
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
        public async Task<IActionResult> PostClientScopes(string clientId, [FromBody] ClientScopeRequest request)
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
                    var temp = await _context.ClientScopes.FirstOrDefaultAsync(x => x.Scope == request.Scope);
                    if (temp != null)
                        return BadRequest();
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
                    var temp = await _context.ClientScopes.FirstOrDefaultAsync(x => x.Scope == request.Scope);
                    if (temp != null)
                        return BadRequest();
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
        [HttpDelete("{clientId}/settings/scopes/{scopeName}")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> DeleteClientScope(string clientId, string scopeName)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return BadRequest();
            var clientScope = await _context.ClientScopes.FirstOrDefaultAsync(x => x.ClientId == client.Id && x.Scope == scopeName);
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
        public async Task<IActionResult> PostClientRedirectUri(string clientId, [FromBody] ClientRedirectUriRequest request)
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
                    var temp = await _context.ClientRedirectUris.FirstOrDefaultAsync(x => x.RedirectUri == request.RedirectUri);
                    if (temp != null)
                        return BadRequest();
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
                    var temp = await _context.ClientRedirectUris.FirstOrDefaultAsync(x => x.RedirectUri == request.RedirectUri);
                    if (temp != null)
                        return BadRequest();
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
        [HttpDelete("{clientId}/settings/redirectUris/redirectUriName")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> DeleteClienRedirectUri(string clientId, string redirectUriName)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return BadRequest();
            var clienRedirectUri = await _context.ClientRedirectUris.FirstOrDefaultAsync(x => x.ClientId == client.Id && x.RedirectUri == redirectUriName);
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
        public async Task<IActionResult> PostClientGrantType(string clientId, [FromBody] ClientGrantTypeRequest request)
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
                    var temp = await _context.ClientGrantTypes.FirstOrDefaultAsync(x => x.GrantType == request.GrantType);
                    if (temp != null)
                        return BadRequest();
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
                    var temp = await _context.ClientGrantTypes.FirstOrDefaultAsync(x => x.GrantType == request.GrantType);
                    if (temp != null)
                        return BadRequest();
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
        [HttpDelete("{clientId}/settings/grantTypes/{grantTypeName}")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> DeleteClientGrantType(string clientId, string grantTypeName)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return BadRequest();
            var clientGrantType = await _context.ClientGrantTypes.FirstOrDefaultAsync(x => x.ClientId == client.Id && x.GrantType == grantTypeName);
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

        // Client Secrets
        [HttpGet("{clientId}/settings/clientSecrets")]
        public async Task<IActionResult> GetClientSecrets(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return NotFound();
            var query = _context.ClientSecrets.Where(x => x.ClientId.Equals(client.Id));
            var clientSecrets = await query.Select(x => new ClientSecretViewModel()
            {
                Id = x.Id,
                Value = x.Value,
                Type = x.Type,
                Expiration = x.Expiration,
                Description = x.Description
            }).ToListAsync();

            return Ok(clientSecrets);
        }

        // Post client secrets
        [HttpPost("{clientId}/settings/clientSecrets")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> PostClientSecret(string clientId, [FromBody] ClientSecretRequest request)
        {
            //Check client
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            //If client not null, Check client Secret
            if (client != null)
            {
                if (request.HashType == "Sha256")
                {
                    var temp = await _context.ClientSecrets.FirstOrDefaultAsync(x => x.Type == request.Type);
                    if (temp != null)
                        return BadRequest();
                    var clientSecretRequest = new IdentityServer4.EntityFramework.Entities.ClientSecret()
                    {
                        Type = request.Type,
                        Value = request.Value.ToSha256(),
                        Description = request.Description,
                        ClientId = client.Id,
                        Expiration = DateTime.Parse(request.Expiration),
                        Created = DateTime.UtcNow
                    };
                    _context.ClientSecrets.Add(clientSecretRequest);
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
                else if (request.HashType == "Sha512")
                {
                    var temp = await _context.ClientSecrets.FirstOrDefaultAsync(x => x.Type == request.Type);
                    if (temp != null)
                        return BadRequest();
                    var clientSecretRequest = new IdentityServer4.EntityFramework.Entities.ClientSecret()
                    {
                        Type = request.Type,
                        Value = request.Value.ToSha256(),
                        Description = request.Description,
                        ClientId = client.Id,
                        Expiration = DateTime.Parse(request.Expiration),
                        Created = DateTime.UtcNow
                    };
                    _context.ClientSecrets.Add(clientSecretRequest);
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

        //Delete client secret
        [HttpDelete("{clientId}/settings/clientSecrets/{secretId}")]
        [RoleRequirement(RoleCode.Admin)]
        public async Task<IActionResult> DeleteClientSecret(string clientId, int secretId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return NotFound();
            var clientSecret = await _context.ClientSecrets.FirstOrDefaultAsync(x => x.ClientId == client.Id && x.Id == secretId);
            if (clientSecret == null)
                return NotFound();
            _context.ClientSecrets.Remove(clientSecret);
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
