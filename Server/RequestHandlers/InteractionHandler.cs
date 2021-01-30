using Server.Model;
using Shared;
using Shared.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.RequestHandlers
{
    public class InteractionHandler : Handler<InteractionRequest>
    {
        private Authenticator authenticator;
        private GameDb context;

        public InteractionHandler(Authenticator authenticator, GameDb context)
        {
            this.authenticator = authenticator;
            this.context = context;
        }

        public override Response Handle(InteractionRequest request)
        {
            int? playerid = authenticator.VerifySession(request.Name, request.SessionId);

            Interactable interactable = context.Interactables.Find(request.InteractableId);
            Player player = context.Players.Find(playerid);

            Response result = interactable.Interact(player, context);
            context.SaveChanges();

            return result;
        }
    }
}
