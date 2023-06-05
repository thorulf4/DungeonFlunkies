using Server.Application.Combat.Skills;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Enemies
{
    public class RatFactory
    {
        private static DamageSkill attackSkill = new DamageSkill("Nibble", 7);

        public static Enemy Create(Encounter encounter)
        {
            List<LoadedSkill> skills = new List<LoadedSkill>() { encounter.LoadSkill(attackSkill, 1) };

            return new Enemy("Rat", 1, skills);
        }
    }
}
