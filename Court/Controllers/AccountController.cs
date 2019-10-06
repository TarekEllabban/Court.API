using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Court.API.IServices;
using Court.Entities.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Court.API.Controllers
{
    public class AccountController : Controller
    {
        private IAccountService _accountService;
        public AccountController(IAccountService accountService = null)
        {
            this._accountService = accountService;
        }
        [Route("api/account/register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand registerUserCommand)
        {
            var isRegisteredSuccessfully = await _accountService.RegisterUser(registerUserCommand);
            if (isRegisteredSuccessfully)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }
    }
}