using Microsoft.EntityFrameworkCore;
using Server.Application;
using Server.Application.Character;
using Server.Application.GameWorld;
using Server.Interactables;
using Server.Model;
using Shared;
using Shared.Requests.Rooms;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.RequestHandlers.Rooms
{
    public class GetRoomHandler : RequestHandler<GetRoomRequest>
    {
        private Mediator mediator;
        private GameDb context;
        private Authenticator authenticator;

        public GetRoomHandler(Mediator mediator, GameDb context, Authenticator authenticator)
        {
            this.mediator = mediator;
            this.context = context;
            this.authenticator = authenticator;
        }

        public override Response Handle(GetRoomRequest request)
        {
            int playerId = authenticator.VerifySession(request.Name, request.SessionId);

            Player player = mediator.GetHandler<GetPlayer>().Get(playerId);

            string[] peopleInRoom = mediator.GetHandler<GetPlayer>().GetInRoom(player.LocationId).Select(p => p.Name).ToArray();
            var interactables = mediator.GetHandler<World>().GetRoom(player).Interactables;

            return Response.From(new RoomResponse
            {
                RoomId = player.LocationId,
                Interactions = interactables.Select(i => i.GetDescriptor(context)).ToArray(),
                PeopleInRoom = peopleInRoom
            });
        }
    }
}
