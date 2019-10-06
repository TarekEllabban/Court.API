using Court.API.IServices;
using Court.Entities.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Court.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [AllowAnonymous]
        [HttpPost, Route("token")]
        public async Task<IActionResult> RequestToken([FromBody] TokenRequestCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _authenticationService.GetUserToken(request);
            if(token != null)
            {
                return Ok(token);
            }

            return Unauthorized("Use is not exist");
        }
    }
}
