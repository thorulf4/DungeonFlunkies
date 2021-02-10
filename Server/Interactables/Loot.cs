using Server.Model;
using Shared;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Shared.Responses;
using Server.Application;
using Shared.Descriptors;

namespace Server.Interactables
{
    public class Loot : Interactable
    {
        public override InteractionDescriptor GetDescriptor(GameDb context)
        {
            return new InteractionDescriptor
            {
                ActionName = "Loot",
                Id = Id
            };
        }

        public override Response Interact(Player player, GameDb context, Mediator mediator)
        {
            return Response.From(new LootResponse());
        }
    }
}
