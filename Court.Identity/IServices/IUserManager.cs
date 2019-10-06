using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Court.Identity.IServices
{
    public interface IUserManager
    {
        Task<bool> CreateUser(IdentityUser user, string password);
        Task<bool> FindUserByEmailAndPassword(string email, string password);
    }
}
