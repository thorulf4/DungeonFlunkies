using Shared;

namespace Server.Model.Items
{
    public class Equipment : Item
    {
        public EquipmentType Type { get; set; }
        public string EquipmentTemplate { get; set; }
        public int ItemPower { get; set; }
    }
}
