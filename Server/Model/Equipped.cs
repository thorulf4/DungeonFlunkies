using Server.Model.Items;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model
{
    public class Equipped : Entity
    {
        public Item Item { get; set; }
        public int ItemId { get; set; }

        public EquipmentType EquipmentType { get; set; }
        public int Slot { get; set; }

        public Player Player { get; set; }
        public int PlayerId { get; set; }
    }
}
