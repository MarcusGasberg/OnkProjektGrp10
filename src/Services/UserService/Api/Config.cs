using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Api
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };


        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("taxingController", "Taxing Controller"),
                new ApiResource("paymentController", "Payment Controller"),
                new ApiResource("stockMarketController", "Stock Market Controller")
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "angularClient",
                    ClientName = "Angular Client",
                    ClientUri = "http://localhost:4200",

                    AllowedGrantTypes = GrantTypes.Implicit,

                    RequireClientSecret = false,
                    RequireConsent = false,
                    RequirePkce = true,

                    RedirectUris =
                    {
                        "http://localhost:4200",
                        "http://localhost:4200/callback",
                    },

                    PostLogoutRedirectUris = { "http://localhost:4200" },
                    AllowedCorsOrigins = { "http://localhost:4200" },

                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600,

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "taxingController",
                        "paymentController",
                        "stockMarketController"
                    }
                }
            };
    }
}
