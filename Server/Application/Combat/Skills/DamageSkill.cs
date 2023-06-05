using Server.Application.Combat;
using Server.Application.Combat.Effects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application.Combat.Skills
{
    public class DamageSkill : Skill
    {
        public float DamageRatio { get; set; }
        public Func<Effect> EffectProvider { get; set; }

        public override TargetType TargetType => TargetType.Enemies;

        public DamageSkill(string name, float damageRatio) : base(name)
        {
            DamageRatio = damageRatio;
        }

        public override void Apply(Encounter encounter, CombatEntity user, CombatEntity target, int ItemPower)
        {
            int damage = (int)(ItemPower * DamageRatio);

            damage = user.modifiers.ModifyOutgoingDamage(damage);

            target.TakeDamage(damage);


            //foreach (OnHitEffect effect in target.GetEffects<OnHitEffect>())
            //    effect.OnHitTriggered(target);

            if (EffectProvider != null)
                target.AddEffect(EffectProvider.Invoke());
        }
    }
}
