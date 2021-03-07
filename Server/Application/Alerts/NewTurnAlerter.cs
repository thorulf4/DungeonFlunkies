using Server.Application.Combat;
using Shared.Alerts.Combat;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Alerts
{
    public class NewTurnAlerter : IApplicationLogic
    {

        private readonly IAlerter alerter;

        public NewTurnAlerter(IAlerter alerter)
        {
            this.alerter = alerter;
        }

        public void SendToAll(Encounter encounter)
        {
            var newTurnAlert = new NewTurnAlert(encounter.enemyTeam.Where(e => e.alive).Select(e => e.GetDescriptor()).ToList(),
                                                encounter.playerTeam.Where(e => e.alive).Select(p => p.GetDescriptor()).ToList());

            alerter.SendAlerts(newTurnAlert, encounter.playerTeam.Select(p => p.name).ToArray());
        }

    }
}
