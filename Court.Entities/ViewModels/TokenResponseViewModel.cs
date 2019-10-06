using System;
using System.Collections.Generic;
using System.Text;

namespace Court.Entities.ViewModels
{
    public class TokenResponseViewModel
    {
        public TokenResponseViewModel()
        {

        }
        public TokenResponseViewModel(string token, int expiration)
        {
            this.Token = token;
            this.Expiration = expiration;

        }
        public string Token { get; set; }
        public int Expiration { get; set; }
    }
}
