using Microsoft.EntityFrameworkCore;
using Server.Application;
using Server.Application.Alerts;
using Server.Application.Character;
using Shared;
using Shared.Alerts;
using Shared.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.RequestHandlers
{
    public class SayHandler : RequestHandler<SayRequest>
    {
        private readonly Mediator mediator;
        private readonly Authenticator authenticator;

        public SayHandler(Mediator mediator, Authenticator authenticator)
        {
            this.mediator = mediator;
            this.authenticator = authenticator;
        }

        public override Response Handle(SayRequest request)
        {
            int playerId = authenticator.VerifySession(request.Name, request.SessionId);

            var roomId = mediator.GetHandler<GetPlayer>().Get(playerId).LocationId;
            mediator.GetHandler<SayAlerter>().SendMessage(roomId, request.Name, request.Message);

            return Response.Ok();
        }
    }
}
