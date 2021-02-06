using Server.Application.Character;
using Shared.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Alerts
{
    public class SayAlerter : IApplicationLogic
    {
        private readonly Mediator mediator;
        private readonly IAlerter alerter;

        public SayAlerter(Mediator mediator, IAlerter alerter)
        {
            this.mediator = mediator;
            this.alerter = alerter;
        }

        public void SendMessage(int roomId, string sender, string message)
        {
            var players = mediator.GetHandler<GetPlayer>().GetInRoom(roomId).Select(p => p.Name).ToList();

            alerter.SendAlerts(new MessageAlert(sender, message), players);
        }
    }
}
