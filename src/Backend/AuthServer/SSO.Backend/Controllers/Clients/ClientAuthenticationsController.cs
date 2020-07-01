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
        #region Client Authentication
        //Get Authentication infor client for edit
        [HttpGet("{clientId}/authentications")]
        [ClaimRequirement(PermissionCode.SSO_VIEW)]
        public async Task<IActionResult> GetClientAuthentication(string clientId)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return NotFound();
            var postLogoutRedirectUris = await _context.ClientPostLogoutRedirectUris
               .Where(x => x.ClientId == client.Id)
               .Select(x => x.PostLogoutRedirectUri.ToString()).ToListAsync();
            var clientAuthenticationViewModel = new ClientAuthenticationViewModel()
            {
                EnableLocalLogin = client.EnableLocalLogin,
                PostLogoutRedirectUris = postLogoutRedirectUris,
                FrontChannelLogoutUri = client.FrontChannelLogoutUri,
                FrontChannelLogoutSessionRequired = client.FrontChannelLogoutSessionRequired,
                BackChannelLogoutUri = client.BackChannelLogoutUri,
                BackChannelLogoutSessionRequired = client.BackChannelLogoutSessionRequired,
                UserSsoLifetime = client.UserSsoLifetime

            };
            return Ok(clientAuthenticationViewModel);
        }

        //Edit authentication infor
        [HttpPut("{clientId}/authentications")]
        [ClaimRequirement(PermissionCode.SSO_UPDATE)]
        public async Task<IActionResult> PutGetClientAuthentication(string clientId, [FromBody] ClientAuthenticationRequest request)
        {
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return NotFound();
            //Table Clients
            client.EnableLocalLogin = request.EnableLocalLogin;
            client.FrontChannelLogoutUri = request.FrontChannelLogoutUri;
            client.FrontChannelLogoutSessionRequired = request.FrontChannelLogoutSessionRequired;
            client.BackChannelLogoutUri = request.BackChannelLogoutUri;
            client.BackChannelLogoutSessionRequired = request.BackChannelLogoutSessionRequired;
            client.UserSsoLifetime = request.UserSsoLifetime;

            _configurationDbContext.Update(client);
            var result = await _configurationDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest();
        }

        //Post Client LogoutRedirectUri for client
        [HttpPost("{clientId}/authentications/postLogoutRedirectUris")]
        [ClaimRequirement(PermissionCode.SSO_CREATE)]
        public async Task<IActionResult> PostClientPostClientRedirectUri(string clientId, [FromBody] ClientPostClientRedirectUriRequest request)
        {
            //Check Client
            var client = await _configurationDbContext.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            //If client not null, Check Client LogoutRedirectUri
            if (client != null)
            {
                var clientPostClientRedirectUri = await _context.ClientPostLogoutRedirectUris.FirstOrDefaultAsync(x => x.ClientId == client.Id);
                //If Client LogoutRedirectUri is null, add LogoutRedirectUri for client
                if (clientPostClientRedirectUri == null)
                {
                    var temp = await _context.ClientPostLogoutRedirectUris.FirstOrDefaultAsync(x => x.PostLogoutRedirectUri == request.PostLogoutRedirectUri);
                    if (temp != null)
                        return BadRequest();
                    var clientPostClientRedirectUriRequest = new IdentityServer4.EntityFramework.Entities.ClientPostLogoutRedirectUri()
                    {
                        PostLogoutRedirectUri = request.PostLogoutRedirectUri,
                        ClientId = client.Id
                    };
                    _context.ClientPostLogoutRedirectUris.Add(clientPostClientRedirectUriRequest);
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
                // If Client LogoutRedirectUri not null, Check Client LogoutRedirectUri on table with request LogoutRedirectUri
                else if (clientPostClientRedirectUri != null)
                {
                    if (clientPostClientRedirectUri.PostLogoutRedirectUri == request.PostLogoutRedirectUri)
                        return BadRequest($"Client PostLogoutRedirectUri {request.PostLogoutRedirectUri} already exist");
                    var temp = await _context.ClientPostLogoutRedirectUris.FirstOrDefaultAsync(x => x.PostLogoutRedirectUri == request.PostLogoutRedirectUri);
                    if (temp != null)
                        return BadRequest();
                    var clientPostClientRedirectUriRequest = new IdentityServer4.EntityFramework.Entities.ClientPostLogoutRedirectUri()
                    {
                        PostLogoutRedirectUri = request.PostLogoutRedirectUri,
                        ClientId = client.Id
                    };
                    _context.ClientPostLogoutRedirectUris.Add(clientPostClientRedirectUriRequest);
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
        //Delete Client LogoutRedirectUris 
        [HttpDelete("{clientId}/authentications/postLogoutRedirectUris/postLogoutRedirectUriName")]
        [ClaimRequirement(PermissionCode.SSO_DELETE)]
        public async Task<IActionResult> DeleteClientPostLogoutRedirectUri(string clientId, string postLogoutRedirectUriName)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
                return BadRequest();
            var clientPostLogoutRedirectUri = await _context.ClientPostLogoutRedirectUris.FirstOrDefaultAsync(x => x.ClientId == client.Id && x.PostLogoutRedirectUri == postLogoutRedirectUriName);
            if (clientPostLogoutRedirectUri == null)
                return NotFound();
            _context.ClientPostLogoutRedirectUris.Remove(clientPostLogoutRedirectUri);
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
