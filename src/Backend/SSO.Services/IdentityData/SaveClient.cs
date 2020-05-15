using IdentityServer4;
using IdentityServer4.Models;
using SSO.Service.CreateModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SSO.Services.IdentityData
{
    public class SaveClient
    {
        public static IEnumerable<Client> ISaveClient(ClientCreateRequest request)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = request.ClientId,
                    ClientName = request.ClientName,
                    Description = request.Description,
                    AllowOfflineAccess = true,
                    LogoUri = request.LogoUri,

                    //Swagger chỉ sử dụng Implicit, sau này các client khác dử dụng code
                    //RequireClientSecret = false,
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
                        "sso.api"
                    }
                }

            };
        }
        }
}
