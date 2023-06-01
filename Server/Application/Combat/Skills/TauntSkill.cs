using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Skills
{
    public class TauntSkill : Skill
    {
        public TauntSkill(string name) : base(name)
        {
        }

        public override TargetType TargetType => TargetType.Self;

        public override void Apply(Encounter encounter, CombatEntity target, int ItemPower)
        {
            foreach(Enemy enemy in encounter.GetAliveAi())
            {
                if(enemy.plannedAction?.skill.skill.TargetType == TargetType.Enemies)
                {
                    enemy.plannedAction.target = target;
                }
            }
        }
    }
}
