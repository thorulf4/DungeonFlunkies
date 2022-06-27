using Server.Model.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Combat.AI
{
    public static class AiController
    {
        public static void SetNextAction(CombatEntity entity, List<CombatEntity> entityAllies , List<CombatEntity> entityEnemies)
        {
            //Consider adding 50% health check before healing

            var skills = entity.skills.Where(s => !s.OnCooldown).ToArray();
            Random random = new Random();

            int skillIndex = random.Next(skills.Length);

            var skill = skills[skillIndex];

            if(skill is DamageSkill)
            {
                int target = entityEnemies[random.Next(entityEnemies.Count)].Id;


                
                Action action = new Action(skill, target);
                //Convert to model.skill not skill descriptor :/
            }
        }
    }
}
