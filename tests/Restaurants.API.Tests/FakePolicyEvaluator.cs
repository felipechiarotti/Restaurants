﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Restaurants.API.Tests
{
    public class FakePolicyEvaluator : IPolicyEvaluator
    {
        public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, "Admin"),
                }));

            var ticket = new AuthenticationTicket(claimsPrincipal, "Bearer");
            var result = AuthenticateResult.Success(ticket);
            return Task.FromResult(result);
        }

        public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context, object? resource)
        {
            var result = PolicyAuthorizationResult.Success();
            return Task.FromResult(result);
        }
    }
}
