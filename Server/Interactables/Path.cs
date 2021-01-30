using Microsoft.EntityFrameworkCore;
using Server.Model;
using Shared;
using Shared.Model;
using Shared.Model.Interactables;
using Shared.Requests;
using Shared.Requests.Interactables;
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

        public override Response Interact(Player player, GameDb context)
        {
            player.Location = LeadsTo;

            context.SaveChanges();

            context.Interactables.Where(i => i.Id == Id).Include("LeadsTo");
            InteractionDescriptor[] descriptors = context.Interactables.Where(i => i.RoomId == LeadsTo.Id).Select(i=> i.GetDescriptor(context)).ToArray();
            string[] names = context.Players.Where(p => p.Location.Id == LeadsTo.Id).Select(p => p.Name).ToArray();

            return Response.From(new RoomResponse
            {
                RoomId = LeadsTo.Id,
                Interactions = descriptors,
                PeopleInRoom = names
            });
        }
    }
}
