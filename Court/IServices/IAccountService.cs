using Court.Entities.Commands;
using Court.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Court.API.IServices
{
    public interface IAccountService
    {
        Task<ResponseViewModel> RegisterUser(RegisterUserCommand registerUserCommand);
    }
}
