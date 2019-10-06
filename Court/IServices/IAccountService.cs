using Court.Entities.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Court.API.IServices
{
    public interface IAccountService
    {
        Task<bool> RegisterUser(RegisterUserCommand registerUserCommand);
    }
}
