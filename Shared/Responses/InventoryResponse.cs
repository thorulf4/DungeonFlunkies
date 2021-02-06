using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Responses
{
    public class InventoryResponse
    {
        public List<Descriptor> Inventory { get; set; }
        public List<Descriptor> DroppedItems { get; set; }
        public Dictionary<EquipmentType, Dictionary<int, EquipmentDescriptor>> EquippedItems { get; set; }
    }
}
