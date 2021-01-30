using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Requests
{
    public class SayRequest : AuthenticatedRequest
    {
        public string Message { get; set; }
    }
}
