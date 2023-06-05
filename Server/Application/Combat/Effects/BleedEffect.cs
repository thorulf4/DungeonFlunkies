using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Effects
{
    public class BleedEffect : Effect
    {
        public int DamagePerTurn { get; set; }

        public BleedEffect()
        {
        }

        public override void OnEffectEnd(CombatEntity entity)
        {

        }

        public override void OnEffectStart(CombatEntity entity)
        {

        }

        public override void Tick(CombatEntity entity)
        {
            entity.TakeDamage(DamagePerTurn);
        }
    }
}
