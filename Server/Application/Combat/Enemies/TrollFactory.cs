using Server.Application.Combat.Skills;
using System;
using System.Collections.Generic;
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

        public static Enemy Create()
        {
            List<LoadedSkill> skills = new List<LoadedSkill>() { new(attackSkill, 1), new LoadedSkill(waitSkill, 1) };

            return new Enemy("Troll", 200, skills);
        }
    }
}
