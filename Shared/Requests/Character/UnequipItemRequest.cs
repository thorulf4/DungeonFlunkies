using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Requests.Character
{
    public class UnequipItemRequest : AuthenticatedRequest
    {
        public EquipmentType Type { get; set; }
        public int Slot { get; set; }
    }
}
