﻿using Server.Model;
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
    class CreateCharacterHandler : Handler<CreateCharacterRequest>
    {
        private GameDb context;
        private Authenticator authenticator;
        private IAlerter alerter;

        public CreateCharacterHandler(GameDb context, Authenticator authenticator, IAlerter alerter)
        {
            this.context = context;
            this.authenticator = authenticator;
            this.alerter = alerter;
        }

        public override Response Handle(CreateCharacterRequest request)
        {
            if (request.Name == "" || request.Secret == "")
                return Response.Fail("Either Name or Password was not given");

            if (context.Players.Any(p => p.Name == request.Name))
                return Response.Fail("Name is already taken");

            Player player = CreateDefaultCharacter(request, context);
            context.Players.Add(player);
            context.SaveChanges();

            string sessionToken = authenticator.CreateSession(request.Name, player.Id);
            alerter.RegisterUser(request.Name);
            return Response.From(sessionToken);
        }

        private static Player CreateDefaultCharacter(CreateCharacterRequest request, GameDb context)
        {
            var player = new Player(request.Name, request.Secret);
            player.Location = context.Rooms.Find(Program.startingRoomId);

            return player;
        }
    }
}
