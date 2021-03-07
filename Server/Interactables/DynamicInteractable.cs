using Server.Application;
using Server.Model;
using Shared;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Interactables
{
    public abstract class DynamicInteractable : IInteractable
    {
        public int Id;

        public abstract DynamicInteractionDescriptor GetDescriptor(GameDb context);
        public abstract Response Interact(Player player, GameDb context, Mediator mediator);

        InteractionDescriptor IInteractable.GetDescriptor(GameDb context)
        {
            return GetDescriptor(context);
        }
    }
}
