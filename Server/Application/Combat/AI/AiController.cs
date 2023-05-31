using Server.Application.Combat.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Application.Combat.AI
{
    public static class AiController
    {
        public static void SetNextAction(Enemy entity, List<Enemy> entityAllies , List<CombatPlayer> players)
        {
            //Consider adding 50% health check before healing
            entity.plannedAction = null;

            Random random = new Random();

            LoadedSkill[] skills = entity.skills.Where(s => !s.OnCooldown).ToArray();
            if (skills.Length == 0)
                return;

            int skillIndex = random.Next(skills.Length);
            LoadedSkill skill = skills[skillIndex];

            if (skill.skill.TargetType == TargetType.Enemies)
            {
                CombatEntity target = players[random.Next(players.Count)];
                
                entity.plannedAction = new PlannedAction(skill, target);
            }else if(skill.skill.TargetType == TargetType.Self)
            {
                entity.plannedAction = new PlannedAction(skill, entity);
            }else if(skill.skill.TargetType == TargetType.Allies)
            {
                CombatEntity target = entityAllies[random.Next(entityAllies.Count)];

                entity.plannedAction = new PlannedAction(skill, target);
            }
            else
            {
                throw new Exception("Enemies only support damage skill atm");
            }
        }
    }
}
