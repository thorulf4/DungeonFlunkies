using Server.Application;
using Server.Application.Character;
using Server.Model;
using Shared;
using Shared.Requests;
using Shared.Requests.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.RequestHandlers
{
    public class LoginHandler : RequestHandler<LoginRequest>
    {
        private Authenticator authenticator;
        private IAlerter alerter;
        private Mediator mediator;

        public LoginHandler(Authenticator authenticator, IAlerter alerter, Mediator mediator)
        {
            this.authenticator = authenticator;
            this.alerter = alerter;
            this.mediator = mediator;
        }

        public override Response Handle(LoginRequest request)
        {
            Player player = mediator.GetHandler<GetPlayer>().Get(request.Name);

            if (player == null || player.Secret != request.Secret)
                return Response.Fail("Invalid name or password");

            string sessionToken = authenticator.CreateSession(request.Name, player.Id);
            alerter.RegisterUser(request.Name);

            return Response.From(sessionToken);
        }
    }
}
