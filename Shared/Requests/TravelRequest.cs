using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Requests
{
    public class TravelRequest : AuthenticatedRequest
    {
        public string TargetLocation { get; set; }
    }
}
