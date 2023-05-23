using Server.Application;
using Server.Application.Character;
using Server.Application.GameWorld;
using Server.Interactables;
using Server.Model;
using Shared;
using Shared.Requests;

namespace Server.RequestHandlers
{
    public class InteractionHandler : RequestHandler<InteractionRequest>
    {
        private readonly GameDb context;
        private readonly Mediator mediator;
        private readonly Authenticator authenticator;

        public InteractionHandler(GameDb context, Mediator mediator, Authenticator authenticator)
        {
            this.context = context;
            this.mediator = mediator;
            this.authenticator = authenticator;
        }


        public override Response Handle(InteractionRequest request)
        {
            int playerid = authenticator.VerifySession(request.Name, request.SessionId);
            Player player = mediator.GetHandler<GetPlayer>().Get(playerid);

            Interactable interactable = mediator.GetHandler<World>().GetRoom(player).GetInteraction(request.Interaction.Id);

            if(interactable != null)
            {
                Response result = interactable.Interact(player, context, mediator);
                context.SaveChanges();
                return result;
            }
            else
            {
                return Response.Fail("No such interactable exists");
            }
        }
    }
}
