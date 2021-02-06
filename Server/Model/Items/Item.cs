using Server.Model.Skills;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model.Items
{
    public class Item : Entity
    {
        public string Name { get; set; }
        public int BaseValue { get; set; }
    }
}
