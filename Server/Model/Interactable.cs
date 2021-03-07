using Server.Application;
using Server.Model;
using Shared;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model
{
    public abstract class Interactable : Entity, IInteractable
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public abstract InteractionDescriptor GetDescriptor(GameDb context);
        public abstract Response Interact(Player player, GameDb context, Mediator mediator);
    }
}
