using Court.API.Extensions;
using Court.Identity.IServices;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Court.API.Services
{
   
    public class ProfileService : IProfileService
    {
        private IUserManager _userManager;
        private IUserClaimsPrincipalFactory<IdentityUser> _claimsFactory;

        public ProfileService(IUserManager userManager, IUserClaimsPrincipalFactory<IdentityUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (context.Subject != null)
            {
                var user = await _userManager.FindUserByIdAsync(context.Subject.GetSubjectId());
                if (user != null)
                {
                    var principal = await _claimsFactory.CreateAsync(user);
                    if (principal != null &&
                        principal.Claims != null && principal.Claims.Any())
                    {
                        var claims = principal.Claims.ToList();
                        var requestedClaimsTypes = context.GetRequestedClaimsTypes();
                        claims = claims.Where(claim => requestedClaimsTypes.Contains(claim.Type)).ToList();
                        context.IssuedClaims.AddRange(claims);   
                    }
                }
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            if (context.Subject != null)
            {
                var user = await _userManager.FindUserByIdAsync(context.Subject.GetSubjectId());
                context.IsActive = (user != null);
            }
        }
    }
}
