using Server.Application;
using Server.Application.Character;
using Server.Model;
using Shared;
using Shared.Model;
using Shared.Requests;
using Shared.Requests.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.RequestHandlers
{
    class CreateCharacterHandler : RequestHandler<CreateCharacterRequest>
    {
        private readonly Mediator mediator;
        private readonly Authenticator authenticator;
        private readonly IAlerter alerter;

        public CreateCharacterHandler(Mediator mediator, Authenticator authenticator, IAlerter alerter)
        {
            this.mediator = mediator;
            this.authenticator = authenticator;
            this.alerter = alerter;
        }

        public override Response Handle(CreateCharacterRequest request)
        {
            if (request.Name == "" || request.Secret == "")
                return Response.Fail("Either Name or Password was not given");

            if (mediator.GetHandler<GetPlayer>().PlayerExists(request.Name))
                return Response.Fail("Name is already taken");

            Player player = mediator.GetHandler<CreateNewCharacter>().CreatePlayer(request.Name, request.Secret);

            string sessionToken = authenticator.CreateSession(request.Name, player.Id);
            alerter.RegisterUser(request.Name);
            return Response.From(sessionToken);
        }


    }
}
