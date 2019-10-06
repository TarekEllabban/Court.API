using Court.Identity.IServices;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Court.Identity.Services
{
    public class UserManager : IUserManager
    {
        private UserManager<IdentityUser> _userManager;
        public UserManager(UserManager<IdentityUser> userManager)
        {
            this._userManager = userManager;
        }
        public async Task<bool> CreateUser(IdentityUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }
        public async Task<bool> FindUserByEmailAndPassword(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var validUserPassword = await _userManager.CheckPasswordAsync(user, password);
                return validUserPassword;
            }
            return false;
        }
    }
}
