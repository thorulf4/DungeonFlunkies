using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model
{
    public class CombatEncounter : Entity
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int CR { get; set; }
    }
}
