using Server.Application.Combat.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Skills
{
    public class StunSkill : DamageSkill
    {
        public int Duration { get; set; }

        public StunSkill(string name, int duration, float damageRation = 0) : base(name, damageRation)
        {
            Duration = duration;
        }

        public override void Apply(Encounter encounter, CombatEntity target, int ItemPower)
        {
            target.AddEffect(new StunEffect(Duration));
            base.Apply(encounter, target, ItemPower);
        }
    }
}
