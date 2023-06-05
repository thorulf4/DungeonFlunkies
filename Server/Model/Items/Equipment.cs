using Shared;

namespace Server.Model.Items
{
    public class Equipment : Item
    {
        public EquipmentType Type { get; set; }
        public string EquipmentTemplate { get; set; }
        public int ItemPower { get; set; }

        public Equipment(string name, EquipmentType type, string equipmentTemplate)
        {
            Name = name;
            Type = type;
            EquipmentTemplate = equipmentTemplate;

            BaseValue = 1;
            ItemPower = 65;
        }
    }
}
