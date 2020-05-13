
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using SSO.Service.CreateModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSO.Backend.Controllers
{
    public class ClientsController : BaseController
    {
        private readonly IClientStore _clientStore;
        private readonly ConfigurationDbContext _configurationDbContext;

        public ClientsController(IClientStore clientStore, ConfigurationDbContext configurationDbContext)
        {
            _clientStore = clientStore;
            _configurationDbContext = configurationDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> PostClient([FromBody]ClientCreateRequest request)
        {
            var clientFind = await _clientStore.FindClientByIdAsync(request.ClientId);
            
            if (clientFind != null)
            {
                return BadRequest($"ClientId {request.ClientId} already exist");
            }
            if(request.AllowedGrantTypes == "Authorization Code Flow")
            {
                foreach (var client in SaveClientAuthorizationCodeFlow(request))
                {
                    _configurationDbContext.Clients.Add(client.ToEntity());
                }
            }
            else if(request.AllowedGrantTypes == "Implicit Flow")
            {
                foreach (var client in SaveClientImplicitFlow(request))
                {
                    _configurationDbContext.Clients.Add(client.ToEntity());
                }
            }else if(request.AllowedGrantTypes == "Hybrid Flow")
            {
                foreach (var client in SaveClientHybridFlow(request))
                {
                    _configurationDbContext.Clients.Add(client.ToEntity());
                }
            }           
            _configurationDbContext.SaveChanges();
            return Ok();
        }
        
        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetClientByClientId(string clientId)
        {
            var client = await _clientStore.FindClientByIdAsync(clientId);

            return Ok(client);
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