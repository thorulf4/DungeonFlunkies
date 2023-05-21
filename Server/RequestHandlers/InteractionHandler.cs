using Microsoft.EntityFrameworkCore;
using Server.Application;
using Server.Application.Alerts;
using Server.Application.Character;
using Server.Application.Interactables;
using Server.Model;
using Shared;
using Shared.Descriptors;
using Shared.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Player player = mediator.GetHandler<GetPlayer>().GetWithLocation(playerid);

            IInteractable interactable;
            if (request.Interaction is DynamicInteractionDescriptor)
            {
                interactable = mediator.GetHandler<DynamicInteractables>().GetForRoom(player.LocationId).FirstOrDefault(i => i.Id == request.Interaction.Id);
            }
            else
            {
                interactable = mediator.GetHandler<GetInteractable>().Get(request.Interaction.Id);

            }

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
