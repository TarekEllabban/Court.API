using System;
using System.Collections.Generic;
using System.Text;

namespace Court.Entities.Commands
{
    public class RegisterUserCommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
