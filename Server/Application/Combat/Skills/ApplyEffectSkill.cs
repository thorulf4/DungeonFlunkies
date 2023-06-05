using Server.Application.Combat.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Skills
{
    public class ApplyEffectSkill : Skill
    {
        private TargetType targetType;
        public Func<Effect> EffectProvider { get; set; }

        public ApplyEffectSkill(string name, TargetType targetType) : base(name)
        {
            this.targetType = targetType;
        }

        public override TargetType TargetType => targetType;

        public override void Apply(Encounter encounter, CombatEntity user, CombatEntity target, int ItemPower)
        {
            Effect effect = EffectProvider.Invoke();
            target.AddEffect(effect);
        }
    }
}
