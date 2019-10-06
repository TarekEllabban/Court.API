using Court.Entities.Commands;
using Court.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Court.API.IServices
{
    public interface IAuthenticationService
    {
        Task<TokenResponseViewModel> GetUserToken(TokenRequestCommand request);
    }
}
