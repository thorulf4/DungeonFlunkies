using Server.Application;
using Shared;
using Shared.Descriptors;

namespace Server.Model
{
    public interface IInteractable
    {
        public abstract Response Interact(Player player, GameDb context, Mediator mediator);

        public abstract InteractionDescriptor GetDescriptor(GameDb context);
    }
}
