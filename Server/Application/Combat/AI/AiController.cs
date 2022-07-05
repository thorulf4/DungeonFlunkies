using Server.Model.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Combat.AI
{
    public static class AiController
    {
        public static void SetNextAction(Enemy entity, List<CombatEntity> entityAllies , List<CombatEntity> entityEnemies)
        {
            //Consider adding 50% health check before healing

            var skills = entity.skills.Where(s => !s.OnCooldown).ToArray();
            Random random = new Random();

            int skillIndex = random.Next(skills.Length);

            LoadedSkill skill = skills[skillIndex];

            if (skill.skill is DamageSkill)
            {
                CombatEntity target = entityEnemies[random.Next(entityEnemies.Count)];
                
                entity.plannedAction = new PlannedAction(skill, target);
            }
            else
            {
                throw new Exception("Enemies only support damage skill atm");
            }
        }
    }
}
