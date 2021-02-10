using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Requests.Character
{
    public class EquipItemRequest : AuthenticatedRequest
    {
        public Descriptor Item { get; set; }
        public EquipmentType Type { get; set; }
        public int Slot { get; set; }
    }
}
