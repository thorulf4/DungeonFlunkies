using Server.Application.Character;
using Server.Model;
using Shared.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Alerts
{
    public class PlayerDiedAlerter : IApplicationLogic
    {
        private readonly IAlerter alerter;
        private readonly Mediator mediator;

        public PlayerDiedAlerter(IAlerter alerter, Mediator mediator)
        {
            this.alerter = alerter;
            this.mediator = mediator;
        }

        public void Alert(string playerName, string reason = "You stubbed your toe")
        {
            alerter.SendAlert(new PlayerDiedAlert(reason), playerName);
        }
    }
}
