using Server.Application.Combat;
using Server.Model.Skills;
using Shared.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Application
{
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

        public static EntityDescriptor GetDescriptor(this CombatEntity entity)
        {
            var descriptor = new EntityDescriptor()
            {
                Id = entity.Id,
                Health = entity.health,
                MaxHealth = entity.maxHealth,
                Name = entity.name,
                Action = null
            };


            if (entity is CombatPlayer player)
            {
                descriptor.HasAction = player.hasAction;
                descriptor.HasBonusAction = player.hasBonusAction;
            }
            else if(entity is Enemy enemy)
            {
                descriptor.Action = enemy.plannedAction.ToString();
            }

            return descriptor;
        }
    }
}
