using Server.Application.Combat.Skills;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Enemies
{
    public class TrollFactory
    {
        private static DamageSkill attackSkill = new DamageSkill("Mega smash", 50)
        {
            Cooldown = 5
        };
        
        private static HealSkill waitSkill = new HealSkill("Rejuvenate", TargetType.Self, 10);

        public static Enemy Create(Encounter encounter)
        {
            List<LoadedSkill> skills = new List<LoadedSkill>() { encounter.LoadSkill(attackSkill, 1), encounter.LoadSkill(waitSkill, 1) };

            return new Enemy("Troll", 200, skills);
        }
    }
}
