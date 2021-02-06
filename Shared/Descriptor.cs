using JsonSubTypes;
using System.Text.Json.Serialization;

namespace Shared
{
    //[JsonConverter(typeof(JsonSubtypes))]
    //[JsonSubtypes.KnownSubTypeWithProperty(typeof(Shared.EquipmentDescriptor), "Slot")]
    //[JsonSubtypes.FallBackSubType(typeof(Shared.ItemDescriptor))]
    public class Descriptor
    {
        public int ItemId { get; set; }

        public string Name { get; set; }
        public int Count { get; set; }

        public Descriptor()
        {

        }

        public Descriptor(int itemId, string name, int count)
        {
            ItemId = itemId;
            Name = name;
            Count = count;
        }

        public override string ToString()
        {
            return $"{Count}X\t{ItemId}: {Name}";
        }
    }
}
