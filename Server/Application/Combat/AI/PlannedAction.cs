using Server.Model.Skills;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application.Combat.AI
{
    public class PlannedAction
    {
        public LoadedSkill skill;
        public int targetId;

        public PlannedAction(LoadedSkill skill, int targetId)
        {
            this.skill = skill;
            this.targetId = targetId;
        }
    }
}
