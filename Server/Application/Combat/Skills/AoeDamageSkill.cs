using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Skills
{
    public class AoeDamageSkill : DamageSkill
    {
        public AoeDamageSkill(string name, float damageRatio) : base(name, damageRatio)
        {
        }

        public override TargetType TargetType => TargetType.Self;

        public override void Apply(Encounter encounter, CombatEntity user, CombatEntity target, int ItemPower)
        {
            foreach(CombatEntity entity in encounter.GetAliveAi())
            {
                base.Apply(encounter, user, entity, ItemPower);
            }
        }
    }
}
