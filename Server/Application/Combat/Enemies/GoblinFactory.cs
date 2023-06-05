using Server.Application.Combat.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Enemies
{
    public class GoblinFactory
    {
        private static DamageSkill attackSkill = new DamageSkill("Bash", 7);

        public static Enemy Create(Encounter encounter)
        {
            List<LoadedSkill> skills = new List<LoadedSkill>() { encounter.LoadSkill(attackSkill, 1) };

            return new Enemy("Goblin", 100, skills);
        }
    }
}
