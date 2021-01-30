using Microsoft.EntityFrameworkCore;
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
    public class GetRoomHandler : Handler<GetRoomRequest>
    {
        private GameDb context;
        private Authenticator authenticator;

        public GetRoomHandler(GameDb context, Authenticator authenticator)
        {
            this.context = context;
            this.authenticator = authenticator;
        }

        public override Response Handle(GetRoomRequest request)
        {
            int? playerId = authenticator.VerifySession(request.Name, request.SessionId);

            int roomId = (from p in context.Players
                          where p.Id == playerId
                          select p.Location.Id).Single();


            string[] peopleInRoom = context.Players.Where(p => p.Location.Id == roomId).Select(p => p.Name).ToArray();
            var interactables = context.Interactables.Where(i => i.RoomId == roomId).ToArray();

            return Response.From(new RoomResponse
            {
                RoomId = roomId,
                Interactions = interactables.Select(i => i.GetDescriptor(context)).ToArray(),
                PeopleInRoom = peopleInRoom
            });
        }
    }
}
