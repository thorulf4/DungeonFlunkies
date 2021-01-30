using Server.Model;
using Shared;
using Shared.Model;
using Shared.Model.Interactables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model
{
    public abstract class Interactable : Entity
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public abstract Response Interact(Player player, GameDb context);

        public abstract InteractionDescriptor GetDescriptor(GameDb context);
    }
}
