using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Application.Combat.Skills
{
    public class RefreshActionSkill : Skill
    {
        public override TargetType TargetType => TargetType.Self;

        public bool RefreshAction { get; set; }
        public bool RefreshBonusAction { get; set; }

        public RefreshActionSkill(string name) : base(name) { }

        public override void Apply(Encounter encounter, CombatEntity target, int ItemPower)
        {
            if(target is CombatPlayer player)
            {
                player.hasAction |= RefreshAction;
                player.hasBonusAction |= RefreshBonusAction;
            }
        }
    }
}
