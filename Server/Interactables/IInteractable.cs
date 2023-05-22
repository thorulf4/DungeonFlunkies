using Server.Application;
using Server.Model;
using Shared;
using Shared.Descriptors;

namespace Server.Interactables
{
    public interface IInteractable
    {
        public abstract Response Interact(Player player, GameDb context, Mediator mediator);

        public abstract InteractionDescriptor GetDescriptor(GameDb context);
    }
}
