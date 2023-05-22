using Server.Application.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application.Combat.Skills
{
    public class DamageSkill : Skill
    {
        public float DamageRatio { get; set; }

        public DamageSkill()
        {

        }

        public DamageSkill(string name, float damageRatio)
        {
            DamageRatio = damageRatio;
            Cooldown = 0;
            UsesAction = true;
            Name = name;
        }

        public override void Apply(CombatEntity target, int ItemPower)
        {
            target.TakeDamage((int)(ItemPower * DamageRatio));
        }
    }
}
