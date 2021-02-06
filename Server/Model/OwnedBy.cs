using Server.Model.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model
{
    public class OwnedBy : Entity
    {
        public Player Owner { get; set; }
        public int OwnerId { get; set; }

        public int Count { get; set; }

        public Item Owns { get; set; }
        public int OwnsId { get; set; }
    }
}
