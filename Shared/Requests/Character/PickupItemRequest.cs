using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Requests.Character
{
    public class PickupItemRequest : AuthenticatedRequest
    {
        public Descriptor Item { get; set; }
    }
}
