// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


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
            };


        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("user_api", "Users Api")
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

                    // where to redirect to after login
                    //RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    //PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>{
                        "user_api",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },

                    AllowOfflineAccess = true
                },
                new Client {
                 ClientId = "angular_spa",
                 ClientName = "Angular SPA",
                 AllowedGrantTypes = GrantTypes.Implicit,
                 AllowedScopes = { "openid", "profile", "email", "user_api" },
                 RedirectUris = {"http://localhost:4200/auth-callback"},
                 PostLogoutRedirectUris = {"http://localhost:4200/"},
                 AllowedCorsOrigins = {"http://localhost:4200"},
                 AllowAccessTokensViaBrowser = true,
                 AccessTokenLifetime = 3600
         }
            };
    }
}