using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Descriptors
{
    public class EquipmentDescriptor : Descriptor
    {
        public EquipmentType Slot { get; set; }

        public EquipmentDescriptor(int itemId, string name, int count) : base(itemId, name, count)
        {
        }

        public EquipmentDescriptor(int itemId, string name, int count, EquipmentType slot) : base(itemId, name, count)
        {
            Slot = slot;
        }

        public EquipmentDescriptor()
        {
        }
    }
}
