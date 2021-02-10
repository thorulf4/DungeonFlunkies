using Shared;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWPF.Utils
{
    public static class Extensions
    {
        public static EquipmentDescriptor GetOrDefault(this Dictionary<EquipmentType, Dictionary<int, EquipmentDescriptor>> dict, EquipmentType type, int slot)
        {
            bool found;
            found = dict.TryGetValue(type, out Dictionary<int, EquipmentDescriptor> slots);
            if (found)
            {
                found = slots.TryGetValue(slot, out EquipmentDescriptor equipment);
                if (found)
                    return equipment;
            }
            return null;
        }
    }
}
