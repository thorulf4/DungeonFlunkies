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
    public class LoginHandler : Handler<LoginRequest>
    {
        private GameDb context;
        private Authenticator authenticator;

        public LoginHandler(GameDb context, Authenticator authenticator)
        {
            this.context = context;
            this.authenticator = authenticator;
        }

        public override Response Handle(LoginRequest request)
        {
            Player player = context.Players.FirstOrDefault(player => player.Name == request.Name);
            
            if(player == null || player.Secret != request.Secret)
                return Response.Fail("Invalid name or password");

            string sessionToken = authenticator.CreateSession(request.Name, player.Id);
            return Response.From(sessionToken);
        }
    }
}
