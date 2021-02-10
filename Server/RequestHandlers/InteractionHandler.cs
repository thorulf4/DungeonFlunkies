using Microsoft.EntityFrameworkCore;
using Server.Application;
using Server.Application.Character;
using Server.Model;
using Shared;
using Shared.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.RequestHandlers
{
    public class InteractionHandler : RequestHandler<InteractionRequest>
    {
        private GameDb context;
        private Mediator mediator;
        private Authenticator authenticator;

        public InteractionHandler(GameDb context, Mediator mediator, Authenticator authenticator)
        {
            this.context = context;
            this.mediator = mediator;
            this.authenticator = authenticator;
        }

        public override Response Handle(InteractionRequest request)
        {
            int playerid = authenticator.VerifySession(request.Name, request.SessionId);

            Interactable interactable = mediator.GetHandler<GetInteractable>().Get(request.InteractableId);
            Player player = mediator.GetHandler<GetPlayer>().GetWithLocation(playerid);

            Response result = interactable.Interact(player, context, mediator);
            context.SaveChanges();

            return result;
        }
    }
}
