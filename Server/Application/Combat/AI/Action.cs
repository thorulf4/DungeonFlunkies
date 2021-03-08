using Server.Model.Skills;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application.Combat.AI
{
    public class Action
    {
        public Skill skill;
        public int targetId;

        public Action(Skill skill, int targetId)
        {
            this.skill = skill;
            this.targetId = targetId;
        }
    }
}
