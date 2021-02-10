﻿using Server.Application.Character;
using Shared.Alerts;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Alerts
{
    public class DroppedItemsAlerter : IApplicationLogic
    {
        private readonly IAlerter alerter;
        private readonly Mediator mediator;

        public DroppedItemsAlerter(IAlerter alerter, Mediator mediator)
        {
            this.alerter = alerter;
            this.mediator = mediator;
        }

        public void Send(int roomId, Descriptor item, params string[] exclude)
        {
            var players = mediator.GetHandler<GetPlayer>().GetInRoom(roomId).Select(p => p.Name).ToArray();

            alerter.SendAlerts(new DroppedItemAlert
            {
                DroppedItem = item
            }, players.Except(exclude).ToArray());
        }
    }
}
