using Server.Model.Skills;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model.Items
{
    public class Equipment : Item
    {
        public EquipmentType Type { get; set; }

        public List<Skill> Skills { get; set; }
    }
}
