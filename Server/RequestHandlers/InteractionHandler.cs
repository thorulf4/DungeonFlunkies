using Microsoft.EntityFrameworkCore;
using Server.Model;
using Shared;
using Shared.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.RequestHandlers
{
    public class InteractionHandler : Handler<InteractionRequest>
    {
        private Authenticator authenticator;
        private GameDb context;
        private IAlerter alerter;

        public InteractionHandler(Authenticator authenticator, GameDb context, IAlerter alerter)
        {
            this.authenticator = authenticator;
            this.context = context;
            this.alerter = alerter;
        }

        public override Response Handle(InteractionRequest request)
        {
            int? playerid = authenticator.VerifySession(request.Name, request.SessionId);

            Interactable interactable = context.Interactables.Find(request.InteractableId);
            Player player = context.Players.Where(p => p.Id == playerid).Include(p => p.Location).Single();

            Response result = interactable.Interact(player, context, alerter);
            context.SaveChanges();

            return result;
        }
    }
}
