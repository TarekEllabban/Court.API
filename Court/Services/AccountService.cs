using Court.API.IServices;
using Court.Entities.Commands;
using Court.Identity.IServices;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Court.API.Services
{
    public class AccountService : IAccountService
    {
        private IUserManager _userManager;
        public AccountService(IUserManager userManager)
        {
            this._userManager = userManager;
        }
        public async Task<bool> RegisterUser(RegisterUserCommand registerUserCommand)
        {
            var user = new IdentityUser();
            user.Email = registerUserCommand.Email;
            user.UserName = registerUserCommand.Email;
            return await _userManager.CreateUser(user, registerUserCommand.Password);
        }
    }
}
