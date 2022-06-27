using Server.Model.Skills;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application.Combat.AI
{
    public class Action
    {
        public SkillDescriptor skill;
        public int targetId;

        public Action(SkillDescriptor skill, int targetId)
        {
            this.skill = skill;
            this.targetId = targetId;
        }
    }
}
