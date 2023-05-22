using Microsoft.EntityFrameworkCore;
using Server.Application;
using Server.Application.Alerts;
using Server.Application.Character;
using Server.Application.GameWorld;
using Server.Model;
using Shared;
using Shared.Alerts;
using Shared.Descriptors;
using Shared.Requests;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Interactables
{
    class Path : Interactable
    {
        public Room LeadsTo { get; set; }

        public Path(Room leadsTo)
        {
            LeadsTo = leadsTo;
        }

        public override InteractionDescriptor GetDescriptor(GameDb context)
        {
            return new InteractionDescriptor
            {
                ActionName = $"Path leading to room X",
                Id = this.Id
            };
        }

        public override Response Interact(Player player, GameDb context, Mediator mediator)
        {
            mediator.GetHandler<RoomUpdateAlerter>().Send(player.LocationId);

            player.LocationId = LeadsTo.Id;
            context.SaveChanges();

            mediator.GetHandler<RoomUpdateAlerter>().Send(LeadsTo.Id);


            string[] peopleInRoom = mediator.GetHandler<GetPlayer>().GetInRoom(LeadsTo.Id).Select(p => p.Name).ToArray();
            var interactables = mediator.GetHandler<World>().GetRoom(player).GetDescriptors(context);

            return Response.From(new RoomResponse
            {
                RoomId = LeadsTo.Id,
                Interactions = interactables,
                PeopleInRoom = peopleInRoom
            });
        }
    }
}
