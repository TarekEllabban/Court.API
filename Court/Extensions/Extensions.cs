using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;

namespace Court.API.Extensions
{
    public static class Extensions
    {
        public static List<string> GetRequestedClaimsTypes(this ProfileDataRequestContext profileDataRequestContext)
        {
            List<string> claims = new List<string>();
            var requestedResources = profileDataRequestContext.RequestedResources;
            if (requestedResources != null)
            {
                #region IdentityResources
                var identityResources = requestedResources.IdentityResources;
                if (identityResources != null)
                {
                    foreach (var resource in identityResources)
                    {
                        if (resource.UserClaims != null && resource.UserClaims.Any())
                        {
                            claims.AddRange(resource.UserClaims);
                        }
                    }
                }
                #endregion

                #region
                var apiResources = requestedResources.ApiResources;
                if (apiResources != null)
                {
                    foreach (var resource in apiResources)
                    {
                        if (resource.UserClaims != null && resource.UserClaims.Any())
                        {
                            claims.AddRange(resource.UserClaims);
                        }
                    }
                }
                #endregion
            }
            return claims;
        }
    }
}
