using Server.Application.Character;
using Shared.Alerts;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Alerts
{
    public class RoomUpdateAlerter : IApplicationLogic
    {
        private readonly GameDb context;
        private readonly IAlerter alerter;
        private readonly Mediator mediator;

        public RoomUpdateAlerter(GameDb context, IAlerter alerter, Mediator mediator)
        {
            this.context = context;
            this.alerter = alerter;
            this.mediator = mediator;
        }

        public void Send(int roomId)
        {
            var players = mediator.GetHandler<GetPlayer>().GetInRoom(roomId).Select(p=>p.Name).ToArray();
            var interactables = mediator.GetHandler<GetInteractable>().GetInRoom(roomId).Select(i => i.GetDescriptor(context)).ToArray();

            alerter.SendAlerts(new RoomAlert { PeopleInRoom = players, Interactions = interactables }, players);
        }
    }
}
