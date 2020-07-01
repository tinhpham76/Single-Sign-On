using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace SSO.BackendIdentityServer
{
    //Seed data identity server if db is null
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResources.Address()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("SSO_API", "SSO API")
                {
                    ApiSecrets = { new Secret("secret".Sha256()) },
                    Scopes = { "SSO_API", "SSO_VIEW", "SSO_CREATE", "SSO_UPDATE" , "SSO_DELETE"}
                },
                 new ApiResource("user.api", "USER API")
                {
                    ApiSecrets = { new Secret("secret".Sha256()) },
                    Scopes = { "user.api" }
                }

            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                // backward compat
                new ApiScope("sso.api"),
                new ApiScope("SSO_VIEW")
                {
                    UserClaims = {"Admin"}
                },
                new ApiScope("SSO_CREATE")
                {
                    UserClaims = {"Admin"}
                },
                new ApiScope("SSO_UPDATE")
                {
                    UserClaims = {"Admin"}
                },
                    new ApiScope("SSO_DELETE")
                {
                    UserClaims = {"Admin"}

                },
                new ApiScope("user.api")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client> {
                new Client
                {
                    ClientId = "swagger_admin",
                    ClientName = "Swagger Admin",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris =           { "https://localhost:5000/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { "https://localhost:5000/swagger" },
                    AllowedCorsOrigins =     { "https://localhost:5000" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "sso.api"
                    }
                },
                 new Client
                {
                    ClientId = "swagger_user_manager",
                    ClientName = "Swagger User Manager",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris =           { "https://localhost:5001/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { "https://localhost:5001/swagger" },
                    AllowedCorsOrigins =     { "https://localhost:5001" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "user.api"
                    }
                },
                new Client
                {
                    ClientName = "Angular Admin",
                    ClientId = "angular_admin",
                    AccessTokenType = AccessTokenType.Reference,
                    RequireConsent = false,

                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,

                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new List<string>
                    {
                        "http://localhost:4200/auth-callback",
                        "http://localhost:4200/silent-renew.html"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:4200/"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:4200"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "sso.api",
                    }
                },
                new Client
                {
                    ClientName = "Angular User Manager",
                    ClientId = "angular_user_manager",
                    AccessTokenType = AccessTokenType.Reference,
                    RequireConsent = false,

                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,

                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new List<string>
                    {
                        "http://localhost:4300/auth-callback",
                        "http://localhost:4300/silent-renew.html"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:4300/"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:4300"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "user.api",
                    }
                }
            };
        }
    }
}