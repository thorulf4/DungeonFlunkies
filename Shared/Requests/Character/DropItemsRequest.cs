using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Requests.Character
{
    public class DropItemsRequest : AuthenticatedRequest
    {
        public List<Descriptor> DroppedItems { get; set; }
    }
}
