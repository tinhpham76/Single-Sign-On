
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Data;
using SSO.Service.CreateModel;
using SSO.Services;
using SSO.Services.CreateModel;
using SSO.Services.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers
{
    public class ClientsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IClientStore _clientStore;
        private readonly ConfigurationDbContext _configurationDbContext;

        public ClientsController(IClientStore clientStore,
            ConfigurationDbContext configurationDbContext,
            ApplicationDbContext context)
        {
            _context = context;
            _clientStore = clientStore;
            _configurationDbContext = configurationDbContext;
        }


        //Đăng ký 1 client mới
        [HttpPost]
        public async Task<IActionResult> PostClient([FromBody]ClientCreateRequest request)
        {
            var clientFind = await _clientStore.FindClientByIdAsync(request.ClientId);

            if (clientFind != null)
            {
                return BadRequest($"ClientId {request.ClientId} already exist");
            }
            if (request.AllowedGrantTypes == "Authorization Code Flow")
            {
                foreach (var client in SaveClientAuthorizationCodeFlow(request))
                {
                    _configurationDbContext.Clients.Add(client.ToEntity());
                }
            }
            else if (request.AllowedGrantTypes == "Implicit Flow")
            {
                foreach (var client in SaveClientImplicitFlow(request))
                {
                    _configurationDbContext.Clients.Add(client.ToEntity());
                }
            }
            else if (request.AllowedGrantTypes == "Hybrid Flow")
            {
                foreach (var client in SaveClientHybridFlow(request))
                {
                    _configurationDbContext.Clients.Add(client.ToEntity());
                }
            }
            _configurationDbContext.SaveChanges();
            return Ok();
        }

        //Get all client
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var clientViewModel = await _context.Clients.Select(u => new ClientQuickView()
            {
                ClientId = u.ClientId,
                ClientName = u.ClientName
            }).ToListAsync();
            return Ok(clientViewModel);
        }
        // Get client với clientId
        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetClientByClientId(string clientId)
        {
            var client = await _clientStore.FindClientByIdAsync(clientId);
            if (client == null)
            {
                return NotFound();
            }
            var clientData = new ClientViewModel()
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                ProtocolType = client.ProtocolType,
                AllowedGrantTypes = client.AllowedGrantTypes,
                RedirectUris = client.RedirectUris,
                PostLogoutRedirectUris = client.PostLogoutRedirectUris,
                AllowOfflineAccess = client.AllowOfflineAccess,
                AllowedCorsOrigins = client.AllowedCorsOrigins,
                AllowedScopes = client.AllowedScopes
            };
            return Ok(clientData);
        }

        //Get client voi pageIndex, page Size
        [HttpGet("filter")]
        public async Task<IActionResult> GetClientsPaging(string filter, int pageIndex, int pageSize)
        {

            if (!string.IsNullOrEmpty(filter))
            {
                var query = _context.Clients.Where(x =>
                x.ClientId.Contains(filter) ||
                x.ClientName.Contains(filter));

                var totalRecords = await _context.Clients.CountAsync();
                var items = await query.Skip((pageIndex - 1 * pageSize))
                    .Take(pageSize)
                    .Select(c => new ClientQuickView()
                    {
                        ClientId = c.ClientId,
                        ClientName = c.ClientName
                    }).ToListAsync();
                var pagination = new Pagination<ClientQuickView>
                {
                    Items = items,
                    TotalRecords = totalRecords,
                };
                return Ok(pagination);
            }
            return BadRequest();

        }
        [HttpPut("{clientId}")]
        public async Task<IActionResult> PutClient(string clientId, [FromBody]ClientUpdateRequest request)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client == null)
            {
                return NotFound();
            }
            var id = client.Id;
            //table client
            client.ClientId = request.ClientId;
            client.ClientName = request.ClientName;
            client.Description = request.Description;
            client.LogoUri = request.LogoUri;
            _context.Clients.Update(client);
            //table RedirectUris
            var redirectUris = await _context.ClientRedirectUris.FirstOrDefaultAsync(x => x.ClientId == id);
            if (redirectUris != null)
                redirectUris.RedirectUri = request.RedirectUris;
            _context.ClientRedirectUris.Update(redirectUris);
            //table RedirectUris
            var clientPostLogoutRedirectUris = await _context.ClientPostLogoutRedirectUris.FirstOrDefaultAsync(x => x.ClientId == id);
            if (clientPostLogoutRedirectUris != null)
                clientPostLogoutRedirectUris.PostLogoutRedirectUri = request.PostLogoutRedirectUris;
            _context.ClientPostLogoutRedirectUris.Update(clientPostLogoutRedirectUris);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
        //Delete client
        [HttpDelete("{clientId}")]
        public async Task<IActionResult> DeleteClient(string clientId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            if (client == null)
            {
                return NotFound($"Can not found client with client id {clientId}");
            }
            //Xoa client
            var id = client.Id;
            _context.Clients.Remove(client);
            //Xoa RedirecUri
            var clientRedirectUri = await _context.ClientRedirectUris.FirstOrDefaultAsync(ru => ru.ClientId == id);
            if (clientRedirectUri != null)
                _context.ClientRedirectUris.Remove(clientRedirectUri);
            //Xoa ClientPostLogoutRedirectUri
            var clientPostLogoutRedirectUri = await _context.ClientPostLogoutRedirectUris.FirstOrDefaultAsync(pl => pl.ClientId == id);
            if (clientPostLogoutRedirectUri != null)
                _context.ClientPostLogoutRedirectUris.Remove(clientPostLogoutRedirectUri);
            //Xoa ClientCorsOrigin
            var clientCorsOrigin = await _context.ClientCorsOrigins.FirstOrDefaultAsync(cc => cc.ClientId == id);
            if (clientCorsOrigin != null)
                _context.ClientCorsOrigins.Remove(clientCorsOrigin);
            //Xoa ClientGrantTypes
            var clientGrantType = await _context.ClientGrantTypes.FirstOrDefaultAsync(pl => pl.ClientId == id);
            if (clientGrantType != null)
                _context.ClientGrantTypes.Remove(clientGrantType);
            //Xoa Scope
            var clientScope = await _context.ClientScopes.FirstOrDefaultAsync(pl => pl.ClientId == id);
            if (clientScope != null)
                _context.ClientScopes.Remove(clientScope);
            //Xoa Secret
            var clientSecret = await _context.ClientSecrets.FirstOrDefaultAsync(pl => pl.ClientId == id);
            if (clientSecret != null)
                _context.ClientSecrets.Remove(clientSecret);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }




        //Save Client

        public static IEnumerable<Client> SaveClientAuthorizationCodeFlow(ClientCreateRequest request)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = request.ClientId,
                    ClientName = request.ClientName,

                    AllowOfflineAccess = request.AllowOfflineAccess,

                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris =           { request.RedirectUris },
                    PostLogoutRedirectUris = { request.PostLogoutRedirectUris },
                    AllowedCorsOrigins =     { request.AllowedCorsOrigins },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        request.AllowedScopes
                    }
                }

            };

        }
        public static IEnumerable<Client> SaveClientImplicitFlow(ClientCreateRequest request)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = request.ClientId,
                    ClientName = request.ClientName,
                    AllowOfflineAccess = request.AllowOfflineAccess,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris =           { request.RedirectUris },
                    PostLogoutRedirectUris = { request.PostLogoutRedirectUris },
                    AllowedCorsOrigins =     { request.AllowedCorsOrigins },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        request.AllowedScopes
                    }
                }

            };

        }
        public static IEnumerable<Client> SaveClientHybridFlow(ClientCreateRequest request)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = request.ClientId,
                    ClientName = request.ClientName,
                    AllowOfflineAccess = request.AllowOfflineAccess,
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris =           { request.RedirectUris },
                    PostLogoutRedirectUris = { request.PostLogoutRedirectUris },
                    AllowedCorsOrigins =     { request.AllowedCorsOrigins },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        request.AllowedScopes
                    }
                }

            };

        }
    }
}