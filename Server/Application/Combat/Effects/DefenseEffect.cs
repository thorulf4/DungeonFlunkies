using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Effects
{
    public class DefenseEffect : Effect
    {
        public float DamageReduction { get; set; }

        public DefenseEffect(int duration, float damageReduction)
        {
            TurnsLeft = duration;
            DamageReduction = damageReduction;
        }

        public override void OnEffectEnd(CombatEntity entity)
        {
            entity.modifiers.DamageReduction -= DamageReduction;
        }

        public override void OnEffectStart(CombatEntity entity)
        {
            entity.modifiers.DamageReduction += DamageReduction;
        }

        public override void Tick(CombatEntity entity)
        {

        }
    }
}
