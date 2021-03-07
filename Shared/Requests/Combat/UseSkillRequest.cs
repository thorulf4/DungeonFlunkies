using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Requests.Combat
{
    public class UseSkillRequest : AuthenticatedRequest
    {
        public SkillDescriptor Skill { get; set; }
        public int TargetId { get; set; }

        public UseSkillRequest(SkillDescriptor skill, int targetId)
        {
            Skill = skill;
            TargetId = targetId;
        }
    }
}
