using Server.Application;
using Server.Application.Combat;
using Shared;
using Shared.Requests.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.RequestHandlers.Combat
{
    public class EndTurnHandler : RequestHandler<EndTurnRequest>
    {
        private readonly Mediator mediator;
        private readonly Authenticator authenticator;

        public EndTurnHandler(Mediator mediator, Authenticator authenticator)
        {
            this.mediator = mediator;
            this.authenticator = authenticator;
        }

        public override Response Handle(EndTurnRequest request)
        {
            int playerId = authenticator.VerifySession(request.Name, request.SessionId);

            mediator.GetHandler<CombatManager>().EndTurn(playerId);

            return Response.Ok();
        }
    }
}
