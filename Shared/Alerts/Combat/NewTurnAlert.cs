using Shared.Descriptors;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Alerts.Combat
{
    public class NewTurnAlert : CombatUpdateAlert
    {
        public NewTurnAlert(List<EntityDescriptor> enemies, List<EntityDescriptor> allies) : base(enemies, allies)
        {
        }
    }
}
