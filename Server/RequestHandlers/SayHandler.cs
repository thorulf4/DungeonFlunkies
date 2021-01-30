using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Alerts;
using Shared.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.RequestHandlers
{
    public class SayHandler : Handler<SayRequest>
    {
        private readonly GameDb context;
        private readonly Authenticator authenticator;
        private readonly IAlerter alerter;

        public SayHandler(GameDb context, Authenticator authenticator, IAlerter alerter)
        {
            this.context = context;
            this.authenticator = authenticator;
            this.alerter = alerter;
        }

        public override Response Handle(SayRequest request)
        {
            var playerId = authenticator.VerifySession(request.Name, request.SessionId);

            var roomId = context.Players.Where(p => p.Id == playerId).Include(p => p.Location).Single().Location.Id;
            var players = context.Players.Where(p => p.Location.Id == roomId).Select(p => p.Name).ToList();

            //temporary testing
            //players.Remove(request.Name);

            alerter.SendAlerts(new MessageAlert(request.Name, request.Message), players);

            return Response.Ok();
        }
    }
}
