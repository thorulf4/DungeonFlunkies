using Server.Model.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model.Skills
{
    public class Skill : Entity
    {
        public Item Item { get; set; }
        public int ItemId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int Cooldown { get; set; }
    }
}
