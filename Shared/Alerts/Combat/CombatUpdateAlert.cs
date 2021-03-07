using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Alerts.Combat
{
    public class CombatUpdateAlert : Alert
    {
        public List<EntityDescriptor> Enemies { get; set; }
        public List<EntityDescriptor> Allies { get; set; }

        public CombatUpdateAlert(List<EntityDescriptor> enemies, List<EntityDescriptor> allies)
        {
            Enemies = enemies;
            Allies = allies;
        }
    }
}
