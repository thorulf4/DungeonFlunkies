using Server.Model.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model
{
    public class Player : Entity
    {
        public Player(string name, string secret)
        {
            Name = name;
            Secret = secret;
            Health = 100;
        }

        //Identification
        public string Name { get; set; }
        public string Secret { get; set; }

        public int Health { get; set; }
        public int LocationId { get; set; }

        public List<OwnedBy> Inventory { get; set; }

        public List<Equipped> Equipment { get; set; }
    }
}
