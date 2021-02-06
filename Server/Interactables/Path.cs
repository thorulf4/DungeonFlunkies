using Microsoft.EntityFrameworkCore;
using Server.Model;
using Shared;
using Shared.Alerts;
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

        public override Response Interact(Player player, GameDb context, IAlerter alerter)
        {
            context.Interactables.Where(i => i.Id == Id).Include("LeadsTo");
            AlertLeaving(player, context, alerter);

            player.Location = LeadsTo;

            context.SaveChanges();

            InteractionDescriptor[] descriptors = context.Interactables.Where(i => i.RoomId == LeadsTo.Id).Select(i => i.GetDescriptor(context)).ToArray();
            string[] names = context.Players.Where(p => p.Location.Id == LeadsTo.Id).Select(p => p.Name).ToArray();

            alerter.SendAlerts(new RoomAlert
            {
                Interactions = descriptors,
                PeopleInRoom = names
            }, names.Except(new string[] { player.Name }).ToArray());

            return Response.From(new RoomResponse
            {
                RoomId = LeadsTo.Id,
                Interactions = descriptors,
                PeopleInRoom = names
            });
        }

        private void AlertLeaving(Player player, GameDb context, IAlerter alerter)
        {
            InteractionDescriptor[] descriptors = context.Interactables.Where(i => i.RoomId == player.Location.Id).Select(i => i.GetDescriptor(context)).ToArray();
            List<string> names = context.Players.Where(p => p.Location.Id == player.Location.Id).Select(p => p.Name).ToList();
            names.Remove(player.Name);

            alerter.SendAlerts(new RoomAlert
            {
                Interactions = descriptors,
                PeopleInRoom = names.ToArray()
            }, names);
        }
    }
}
