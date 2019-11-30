using System;
using System.Collections.Generic;
using System.Text;

namespace Court.Entities.ViewModels
{
    public class ResponseViewModel
    {
        public bool IsSucceeded { get; set; }
        public string Data { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
    }
}
