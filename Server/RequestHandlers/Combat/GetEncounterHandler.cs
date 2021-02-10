using Server.Application;
using Server.Application.Combat;
using Shared;
using Shared.Descriptors;
using Shared.Requests.Combat;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.RequestHandlers.Combat
{
    public class GetEncounterHandler : RequestHandler<GetEncounterRequest>
    {
        private readonly Mediator mediator;
        private readonly Authenticator authenticator;

        public GetEncounterHandler(Mediator mediator, Authenticator authenticator)
        {
            this.mediator = mediator;
            this.authenticator = authenticator;
        }

        public override Response Handle(GetEncounterRequest request)
        {
            int playerId = authenticator.VerifySession(request.Name, request.SessionId);

            var encounter = mediator.GetHandler<GetEncounter>().Get(playerId);

            return Response.From(encounter);
        }
    }
}
