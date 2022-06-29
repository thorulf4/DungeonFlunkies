using Server.Application.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model.Skills
{
    public class DamageSkill : Skill
    {
        public int Damage { get; set; }

        public DamageSkill()
        {

        }

        public DamageSkill(string name, int damage)
        {
            Damage = damage;
            Cooldown = 0;
            UsesAction = true;
            Name = name;
        }

        public override void Apply(CombatEntity target)
        {
            target.TakeDamage(Damage);
        }
    }
}
