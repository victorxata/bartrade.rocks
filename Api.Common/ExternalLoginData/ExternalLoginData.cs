using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace Api.Common.ExternalLoginData
{
    public class ExternalLoginData
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        private string UserName { get; set; }
        private string Email { get; set; }

        public IEnumerable<Claim> GetClaims()
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider)
                };

            if (UserName != null)
            {
                claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
            }
            if (Email != null)
            {
                claims.Add(new Claim(ClaimTypes.Email, Email, null, LoginProvider));
            }

            return claims;
        }

        public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
        {
            if (identity == null)
            {
                return null;
            }

            var providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

            if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                || String.IsNullOrEmpty(providerKeyClaim.Value))
            {
                return null;
            }

            if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
            {
                return null;
            }

            return new ExternalLoginData
            {
                LoginProvider = providerKeyClaim.Issuer,
                ProviderKey = providerKeyClaim.Value,
                UserName = identity.FindFirstValue(ClaimTypes.Name),
                Email = identity.FindFirstValue(ClaimTypes.Email)
            };

        }
    }
}
