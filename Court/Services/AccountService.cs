using Court.API.IServices;
using Court.Entities.Commands;
using Court.Entities.ViewModels;
using Court.Identity.IServices;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
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
        public async Task<ResponseViewModel> RegisterUser(RegisterUserCommand registerUserCommand)
        {
            var response = new ResponseViewModel();

            if (!string.IsNullOrEmpty(registerUserCommand.Username) && !string.IsNullOrEmpty(registerUserCommand.Password))
            {
                var user = new IdentityUser();
                user.Email = registerUserCommand.Username;
                user.UserName = registerUserCommand.Username;

                var userExist = await _userManager.FindUserByEmail(registerUserCommand.Username);

                if (userExist == null)
                {
                    response.IsSucceeded = await _userManager.CreateUser(user, registerUserCommand.Password);

                    if (response.IsSucceeded)
                    {
                        var identityUser = await _userManager.FindUserByEmail(registerUserCommand.Username);
                        if (identityUser != null)
                        {
                            var Data = new
                            {
                                IdentityId = identityUser.Id
                            };
                            response.Data = JsonConvert.SerializeObject(Data);
                        }

                    }
                }
                else
                {
                    response.IsSucceeded = false;
                    response.Message = "User is already exist";
                }
            }
            else
            {
                response.IsSucceeded = false;
                response.Message = "Invalid Input";
            }
            return response;
        }

    }
}
