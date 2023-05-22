using Server.Application.Combat;
using Server.Application.Combat.Skills;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application
{
    //TODO this is not a scalable way of adding descriptor conversions
    public static class DescriptorExtensions
    {
        public static SkillDescriptor GetDescriptor(this Skill skill, int cooldown = -1)
        {
            if (cooldown == -1)
                cooldown = skill.Cooldown;

            return new SkillDescriptor()
            {
                Id = skill.Id,
                Name = skill.Name,
                Description = skill.Description,
                Cooldown = skill.Cooldown,
                CurrentCooldown = cooldown,
                UsesAction = skill.UsesAction,
                UsesBonusAction = skill.UsesBonusAction
            };
        }
    }
}
