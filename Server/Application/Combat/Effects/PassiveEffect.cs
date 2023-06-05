using Server.Application.Combat.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Effects
{
    public class PassiveEffect : Effect
    {
        public int FlatDamageIncrease { get; set; }
        public float DamageModifier { get; set; }

        public override void OnEffectEnd(CombatEntity entity)
        {
            entity.modifiers.FlatDamageIncrease -= FlatDamageIncrease;
            entity.modifiers.DamageModifier -= DamageModifier;
        }

        public override void OnEffectStart(CombatEntity entity)
        {
            entity.modifiers.FlatDamageIncrease += FlatDamageIncrease;
            entity.modifiers.DamageModifier += DamageModifier;
        }

        public override void Tick(CombatEntity entity)
        {

        }
    }
}
