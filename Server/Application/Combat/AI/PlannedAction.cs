using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application.Combat.AI
{
    public class PlannedAction
    {
        public LoadedSkill skill;
        public CombatEntity target;

        public PlannedAction(LoadedSkill skill, CombatEntity target)
        {
            this.skill = skill;
            this.target = target;
        }

        public override string ToString()
        {
            return $"{skill.Name} -> {target.name}";
        }

        public void Apply(Encounter encounter, CombatEntity user)
        {
            skill.Apply(encounter, user, target);
        }
    }
}
