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

        public override void Apply(CombatEntity target, int ItemPower)
        {
            var stun = new StunEffect(Duration);
            stun.Tick(target); // Cause effect immedietely

            target.AddEffect(stun);
            base.Apply(target, ItemPower);
        }
    }
}
