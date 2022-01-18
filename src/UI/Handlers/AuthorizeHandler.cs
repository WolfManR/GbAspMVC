﻿using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace UI.Handlers
{
    public class AuthorizeHandler : DelegatingHandler
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthorizeHandler(ProtectedLocalStorage protectedLocalStorage, AuthenticationStateProvider authenticationStateProvider)
        {
            _protectedLocalStorage = protectedLocalStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            if (state.User.Identity?.IsAuthenticated is false or null)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            var token = await _protectedLocalStorage.GetAsync<string>("accessToken");
            if(!token.Success) return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}