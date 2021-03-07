using Server.Application.Combat;
using Shared.Alerts.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Alerts
{
    public class CombatUpdateAlerter : IApplicationLogic
    {
        private readonly IAlerter alerter;

        public CombatUpdateAlerter(IAlerter alerter)
        {
            this.alerter = alerter;
        }

        public void SendToAll(Encounter encounter)
        {
            var updateAlert = new CombatUpdateAlert(encounter.enemyTeam.Where(e => e.alive).Select(e => e.GetDescriptor()).ToList(),
                                                encounter.playerTeam.Where(e => e.alive).Select(p => p.GetDescriptor()).ToList());

            alerter.SendAlerts(updateAlert, encounter.playerTeam.Select(p => p.name).ToArray());
        }
    }
}
