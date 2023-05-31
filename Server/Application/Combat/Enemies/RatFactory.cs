using Server.Application.Combat.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Enemies
{
    public class RatFactory
    {
        private static DamageSkill attackSkill = new DamageSkill("Nibble", 3);

        public static Enemy Create()
        {
            List<LoadedSkill> skills = new List<LoadedSkill>() { new(attackSkill, 1) };

            return new Enemy("Rat", 1, skills);
        }
    }
}
