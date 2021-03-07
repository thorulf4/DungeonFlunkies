using Server.Application.Combat;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model.Skills
{
    public class DamageSkill : Skill
    {
        public int Damage { get; set; }

        public override void Apply(CombatEntity target)
        {
            target.TakeDamage(Damage);
        }
    }
}
