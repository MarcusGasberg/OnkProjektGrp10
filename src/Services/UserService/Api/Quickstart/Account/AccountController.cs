// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Quickstart.UI
{
    /// <summary>
    /// This sample controller implements a typical login/logout/provision workflow for local and external accounts.
    /// The login service encapsulates the interactions with the user data store. This data store is in-memory only and cannot be used for production!
    /// The interaction service provides a way for the UI to communicate with identityserver for validation and context retrieval
    /// </summary>
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly TestUserStore _users;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            TestUserStore users = null)
        {
            // if the TestUserStore is not in DI, then we'll just use the global users collection
            // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
            _users = users ?? new TestUserStore(TestUsers.Users);

            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // build a model so we know what to show on the login page
            var vm = await BuildLoginViewModelAsync(returnUrl);

            return View(vm);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button != "login")
            {
                return await HandleCancel(context, model);
            }

            if (!ModelState.IsValid)
            {
                // something went wrong, show form with error
                var vm = await BuildLoginViewModelAsync(model);
                return View(vm);
            }

            if (!_users.ValidateCredentials(model.Username, model.Password))
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);

                return Redirect(model.ReturnUrl ?? "~/");
            }

            var user = _users.FindByUsername(model.Username);
            await _events.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.SubjectId, user.Username, clientId: context?.ClientId));

            // only set explicit expiration here if user chooses "remember me". 
            // otherwise we rely upon expiration configured in cookie middleware.
            AuthenticationProperties props = null;
            if (AccountOptions.AllowRememberLogin && model.RememberLogin)
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                };
            };

            // issue authentication cookie with subject ID and username
            await HttpContext.SignInAsync(user.SubjectId, user.Username, props);

            if (context != null)
            {
                if (await _clientStore.IsPkceClientAsync(context.ClientId))
                {
                    // if the client is PKCE then we assume it's native, so this change in how to
                    // return the response is for better UX for the end user.
                    return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                }

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                return Redirect(model.ReturnUrl);
            }

            // request for a local page
            if (Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl ?? "~/");
            }
            else
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }
        }

        private async Task<IActionResult> HandleCancel(AuthorizationRequest context, LoginInputModel model)
        {
            if (context != null)
            {
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                if (await _clientStore.IsPkceClientAsync(context.ClientId))
                {
                    // if the client is PKCE then we assume it's native, so this change in how to
                    // return the response is for better UX for the end user.
                    return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                }

                return Redirect(model.ReturnUrl);
            }
            else
            {
                // since we don't have a valid context, then we just go back to the home page
                return Redirect("~/");
            }
        }


        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = new LogoutInputModel() { LogoutId = logoutId };

            return await Logout(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                await HttpContext.SignOutAsync();

                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }
            var logout = await _interaction.GetLogoutContextAsync(model.LogoutId);

            return Redirect(logout.PostLogoutRedirectUri);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            var allowLocal = true;

            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }
    }
}
