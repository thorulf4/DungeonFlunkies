using Server.Application.Combat.Skills;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Enemies
{
    public class RatmancerFactory
    {
        private static DamageSkill attackSkill = new DamageSkill("Staff strike", 10);

        private static SpawnHelperSkill spawnSkill = new SpawnHelperSkill("Call rats", RatFactory.Create)
        {
            Cooldown = 3,
            EnemyCount = 2
        };

        public static Enemy Create(Encounter encounter)
        {
            List<LoadedSkill> skills = new List<LoadedSkill>() { encounter.LoadSkill(attackSkill, 1), encounter.LoadSkill(spawnSkill, 1) };

            return new Enemy("Ratmancer", 120, skills);
        }
    }
}
