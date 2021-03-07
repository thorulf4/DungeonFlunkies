using Microsoft.EntityFrameworkCore;
using Server.Application;
using Server.Application.Alerts;
using Server.Application.Character;
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

        public override InteractionDescriptor GetDescriptor(GameDb context)
        {
            context.Interactables.Include("LeadsTo").Single(i => i.Id == Id);

            return new InteractionDescriptor
            {
                ActionName = $"Path leading to room {LeadsTo.Id}",
                Id = this.Id
            };
        }

        public override Response Interact(Player player, GameDb context, Mediator mediator)
        {
            context.Interactables.Where(i => i.Id == Id).Include("LeadsTo");
            mediator.GetHandler<RoomUpdateAlerter>().Send(player.LocationId);

            player.Location = LeadsTo;
            context.SaveChanges();

            mediator.GetHandler<RoomUpdateAlerter>().Send(LeadsTo.Id);


            string[] peopleInRoom = mediator.GetHandler<GetPlayer>().GetInRoom(LeadsTo.Id).Select(p => p.Name).ToArray();
            var interactables = mediator.GetHandler<GetInteractable>().GetInRoom(LeadsTo.Id).ToArray();

            return Response.From(new RoomResponse
            {
                RoomId = LeadsTo.Id,
                Interactions = interactables.Select(i => i.GetDescriptor(context)).ToArray(),
                PeopleInRoom = peopleInRoom
            });
        }
    }
}
