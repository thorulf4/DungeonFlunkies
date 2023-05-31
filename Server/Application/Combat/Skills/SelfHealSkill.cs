using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Skills
{
    public class SelfHealSkill : Skill
    {
        public float HealRatio { get; set; }

        public override TargetType TargetType => TargetType.Self;

        public SelfHealSkill(string name, float healRatio) : base(name)
        {
            HealRatio = healRatio;
            Cooldown = 0;
            UsesAction = true;
        }

        public override void Apply(Encounter encounter, CombatEntity target, int ItemPower)
        {
            target.HealDamage((int)(ItemPower * HealRatio));
        }
    }
}
