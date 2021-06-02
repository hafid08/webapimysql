using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class LoginRequest
    {
        public string puser { get; set; }
        public string ppass { get; set; }
    }
}
