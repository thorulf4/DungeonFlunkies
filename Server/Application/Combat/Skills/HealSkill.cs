using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Skills
{
    public class HealSkill : Skill
    {
        public float HealRatio { get; set; }
        public TargetType targetType;

        public override TargetType TargetType => targetType;

        public HealSkill(string name, TargetType targetType, float healRatio) : base(name)
        {
            HealRatio = healRatio;
            this.targetType = targetType;
            Cooldown = 0;
            UsesAction = true;
        }

        public override void Apply(Encounter encounter, CombatEntity user, CombatEntity target, int ItemPower)
        {
            target.HealDamage((int)(ItemPower * HealRatio));
        }
    }
}
