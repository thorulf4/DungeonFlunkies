using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Requests.Authentication
{
    public class LoginRequest
    {
        public string Name { get; set; }
        public string Secret { get; set; }
    }
}
