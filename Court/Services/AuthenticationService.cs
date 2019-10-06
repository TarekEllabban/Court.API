using Court.API.IServices;
using Court.Entities.Commands;
using Court.Entities.ViewModels;
using Court.Identity.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Court.API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IUserManager _userManager;
        private IConfiguration _configuration;
        public AuthenticationService(IUserManager userManager, IConfiguration configuration)
        {
            this._userManager = userManager;
            this._configuration = configuration;
        }
        public async Task<TokenResponseViewModel> GetUserToken(TokenRequestCommand request)
        {
            TokenResponseViewModel tokenResponseViewModel = null;
            if (await _userManager.FindUserByEmailAndPassword(request.Username, request.Password))
            {
                var claim = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username)
                };
                var tokenManagementViewModel = _configuration.GetSection("TokenManagement").Get<TokenManagementViewModel>();
                var secret = Encoding.ASCII.GetBytes(tokenManagementViewModel.Secret);
                var key = new SymmetricSecurityKey(secret);
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                DateTime expiresAt = DateTime.Now.AddMinutes(tokenManagementViewModel.AccessExpiration);
                var jwtToken = new JwtSecurityToken(
                    tokenManagementViewModel.Issuer,
                    tokenManagementViewModel.Audience,
                    claim,
                    expires: expiresAt,
                    signingCredentials: credentials
                );
                var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                tokenResponseViewModel = new TokenResponseViewModel(token, (int)(expiresAt - new DateTime(1970, 1, 1)).TotalSeconds);
            }
            return tokenResponseViewModel;
        }
    }
}
