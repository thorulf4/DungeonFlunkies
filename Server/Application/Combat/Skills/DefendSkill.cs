using Server.Application.Combat.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Skills
{
    public class DefendSkill : Skill
    {
        public float DamageReduction { get; set; } = 0;
        public int Turns { get; set; }

        public DefendSkill(string name) : base(name)
        {
        }

        public override TargetType TargetType => TargetType.Allies;

        public override void Apply(Encounter encounter, CombatEntity target, int ItemPower)
        {
            target.AddEffect(new DefenseEffect(Turns, DamageReduction));
        }
    }
}
