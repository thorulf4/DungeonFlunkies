using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Skills
{
    public class ReduceCooldownsSkill : Skill
    {
        public int Amount { get; set; }

        public ReduceCooldownsSkill(string name) : base(name)
        {
        }

        public override TargetType TargetType => TargetType.Self;

        public override void Apply(Encounter encounter, CombatEntity user, CombatEntity target, int ItemPower)
        {
            foreach(LoadedSkill skill in target.skills)
            {
                skill.LowerCooldown(Amount);
            }
        }
    }
}
