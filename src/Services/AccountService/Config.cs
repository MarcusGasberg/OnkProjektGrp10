// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace AccountService
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
                new ApiResource("api1", "Test Api"),
                new ApiResource("taxingController", "Taxing Controller"),
                new ApiResource("paymentController", "Payment Controller"),
                new ApiResource("stockMarketController", "Stock Market Controller", new[] { JwtClaimTypes.Profile, JwtClaimTypes.NickName, JwtClaimTypes.Name }),
                new ApiResource("bankController", "Bank Controller", new[] { JwtClaimTypes.Profile, JwtClaimTypes.NickName, JwtClaimTypes.Name })
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // client credentials flow client
                new Client
                {
                    ClientId = "client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = {
                        "api1",
                        "taxingController",
                        "paymentController",
                        "stockMarketController",
                        "bankController",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                     },
                },
                new Client
                {
                    ClientId = "angularDebugClient",
                    ClientName = "Angular Debug Client",
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
                        "api1",
                        "taxingController",
                        "paymentController",
                        "stockMarketController",
                        "bankController",
                    }
                },
                new Client
                {
                    ClientId = "angularDockerClient",
                    ClientName = "Angular Docker Client",
                    ClientUri = "http://192.168.99.100:4200/",

                    AllowedGrantTypes = GrantTypes.Implicit,

                    RequireClientSecret = false,
                    RequireConsent = false,
                    RequirePkce = true,

                    RedirectUris =
                    {
                        "http://192.168.99.100:4200",
                        "http://192.168.99.100:4200/callback",
                    },

                    PostLogoutRedirectUris = { "http://192.168.99.100:4200" },
                    AllowedCorsOrigins = { "http://192.168.99.100:4200", },

                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600,

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api1",
                        "taxingController",
                        "paymentController",
                        "stockMarketController",
                        "bankController",
                    }
                },
                new Client
                {
                    ClientId = "angularClient",
                    ClientName = "Angular Client",
                    ClientUri = "http://web-app.stocks",

                    AllowedGrantTypes = GrantTypes.Implicit,

                    RequireClientSecret = false,
                    RequireConsent = false,
                    RequirePkce = true,

                    RedirectUris =
                    {
                        "http://web-app.stocks",
                        "http://web-app.stocks/callback",
                    },

                    PostLogoutRedirectUris = { "http://web-app.stocks" },
                    AllowedCorsOrigins = { "http://web-app.stocks" },

                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600,

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api1",
                        "taxingController",
                        "paymentController",
                        "stockMarketController",
                        "bankController",
                    }
                }
            };
    }
}