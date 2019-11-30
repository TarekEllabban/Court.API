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
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand registerUserCommand)
        {
            var response = await _accountService.RegisterUser(registerUserCommand);
            return Ok(response);
           
        }
    }
}