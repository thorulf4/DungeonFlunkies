using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Requests
{
    public class AuthenticatedRequest
    {
        public string Name { get; set; }
        public string SessionId { get; set; }
    }
}
